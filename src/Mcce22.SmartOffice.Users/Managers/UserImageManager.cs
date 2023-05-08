using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using AutoMapper;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Users.Entities;
using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Managers
{
    public interface IUserImageManager
    {
        Task<UserImageModel[]> GetUserImages(string userId);

        Task<UserImageModel> StoreUserImage(string userId, IFormFile file);

        Task DeleteUserImage(string userImageId);
    }

    public class UserImageManager : IUserImageManager
    {
        private const string BUCKET_PARAMETER_NAME = "bucketname";

        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly IAmazonS3 _s3Client;
        private readonly IIdGenerator _idGenerator;
        private readonly IAmazonSimpleSystemsManagement _systemsManagement;
        private readonly IMapper _mapper;

        public UserImageManager(
            IAmazonDynamoDB dynamoDbClient,
            IAmazonS3 s3Client,            
            IIdGenerator idGenerator,
            IAmazonSimpleSystemsManagement systemsManagement,
            IMapper mapper)
        {
            _dynamoDbClient = dynamoDbClient;
            _s3Client = s3Client;
            _idGenerator = idGenerator;
            _systemsManagement = systemsManagement;
            _mapper = mapper;
        }

        private async Task<string> GetBucketName()
        {
            var result = await _systemsManagement.GetParameterAsync(new GetParameterRequest
            {
                Name = BUCKET_PARAMETER_NAME
            });

            return result.Parameter.Value;
        }

        public async Task<UserImageModel[]> GetUserImages(string userId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var userImages = await context.QueryAsync<UserImage>(userId, new DynamoDBOperationConfig
            {
                IndexName = $"{nameof(UserImage.UserId)}-index"
            }).GetRemainingAsync();

            return userImages.Select(_mapper.Map<UserImageModel>).ToArray();
        }

        public async Task<UserImageModel> StoreUserImage(string userId, IFormFile file)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var bucketName = await GetBucketName();
            var key = $"{userId}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var utility = new TransferUtility(_s3Client);
            var request = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = file.OpenReadStream(),
                AutoCloseStream = true,
                AutoResetStreamPosition = true
            };

            var userImage = new UserImage
            {
                Id = _idGenerator.GenerateId(),
                ResourceKey = key,
                UserId = userId,
                Url = $"https://{bucketName}.s3.amazonaws.com/{key}",
                Size = file.Length
            };

            await context.SaveAsync(userImage);

            await utility.UploadAsync(request);

            return _mapper.Map<UserImageModel>(userImage);
        }

        public async Task DeleteUserImage(string userImageId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var bucketName = await GetBucketName();
            var userImage = await context.LoadAsync<UserImage>(userImageId);

            if (userImage != null)
            {
                await _s3Client.DeleteObjectAsync(bucketName, userImage.ResourceKey);
                await context.DeleteAsync(userImage);
            }
            else
            {
                var response = await _s3Client.ListObjectsAsync(bucketName, userImageId);

                foreach(var s3Object in response.S3Objects)
                {
                    await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = s3Object.Key
                    });
                }

                var userImages = await context.QueryAsync<UserImage>(userImageId, new DynamoDBOperationConfig
                {
                    IndexName = $"{nameof(UserImage.UserId)}-index"
                }).GetRemainingAsync();

                foreach (var image in userImages)
                {
                    await context.DeleteAsync(image);
                }
            }
        }
    }
}
