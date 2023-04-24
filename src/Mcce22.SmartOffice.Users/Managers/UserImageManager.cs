using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
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
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly IAmazonS3 _s3Client;
        private readonly IIdGenerator _idGenerator;
        private readonly IMapper _mapper;
        private readonly string _bucketName;

        public UserImageManager(
            IAmazonDynamoDB dynamoDbClient,
            IAmazonS3 s3Client,
            IAppSettings appSettings,
            IIdGenerator idGenerator,
            IMapper mapper)
        {
            _dynamoDbClient = dynamoDbClient;
            _s3Client = s3Client;
            _idGenerator = idGenerator;
            _mapper = mapper;
            _bucketName = appSettings.BucketName;
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

            using var ms = new MemoryStream();

            await file.CopyToAsync(ms);

            var key = $"{userId}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var utility = new TransferUtility(_s3Client);
            var request = new TransferUtilityUploadRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = ms,
                AutoCloseStream = true,
                AutoResetStreamPosition = true
            };

            var userImage = new UserImage
            {
                Id = _idGenerator.GenerateId(),
                ResourceKey = key,
                UserId = userId,
                Url = $"https://{_bucketName}.s3.amazonaws.com/{key}"
            };

            await context.SaveAsync(userImage);

            await utility.UploadAsync(request);

            return _mapper.Map<UserImageModel>(userImage);
        }

        public async Task DeleteUserImage(string userImageId)
        {
            using var context = new DynamoDBContext(_dynamoDbClient);

            var userImage = await context.LoadAsync<UserImage>(userImageId);

            if (userImage != null)
            {
                await _s3Client.DeleteObjectAsync(_bucketName, userImage.ResourceKey);
                await context.DeleteAsync(userImage);
            }
            else
            {
                var response = await _s3Client.ListObjectsAsync(_bucketName, userImageId);

                foreach(var s3Object in response.S3Objects)
                {
                    await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
                    {
                        BucketName = _bucketName,
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
