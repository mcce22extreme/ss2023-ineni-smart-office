using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Users.Entities;
using Mcce22.SmartOffice.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Users.Managers
{
    public interface IUserImageManager
    {
        Task<UserImageModel[]> GetUserImages(int userId);

        Task<UserImageModel> CreateUserImage(SaveUserImageModel model);

        Task<UserImageModel> StoreUserImageContent(int userImageId, Stream stream);

        Task DeleteUserImage(int userImageId);

        Task DeleteUserImageOfUser(int userId);
    }

    public class UserImageManager : IUserImageManager
    {
        private readonly string _baseUrl;
        private readonly string _bucketName;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserImageManager(StorageConfiguration storageConfiguration, AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

            _baseUrl = storageConfiguration.BaseUrl;
            _bucketName = storageConfiguration.BucketName;
        }

        private UserImageModel CreateModel(UserImage userImage)
        {
            var model = _mapper.Map<UserImageModel>(userImage);

            model.ResourceUrl = $"{_baseUrl}/{userImage.ResourceKey}";

            return model;
        }

        public async Task<UserImageModel[]> GetUserImages(int userId)
        {
            var userImageQuery = _dbContext.UserImages.AsQueryable();

            if (userId > 0)
            {
                userImageQuery = userImageQuery.Where(x => x.UserId == userId);
            }

            var userImages = await userImageQuery.ToListAsync();

            return userImages.Select(x => _mapper.Map(x, CreateModel(x))).ToArray();
        }

        public async Task<UserImageModel> CreateUserImage(SaveUserImageModel model)
        {
            var userImage = _mapper.Map<UserImage>(model);

            userImage.ResourceKey = $"{Guid.NewGuid()}{Path.GetExtension(model.FileName)}";

            await _dbContext.UserImages.AddAsync(userImage);

            await _dbContext.SaveChangesAsync();

            return CreateModel(userImage);
        }

        public async Task<UserImageModel> StoreUserImageContent(int userImageId, Stream stream)
        {
            var userImage = await _dbContext.UserImages.FirstOrDefaultAsync(x => x.Id == userImageId);

            if (userImage == null)
            {
                throw new NotFoundException($"Could not find user image with id '{userImageId}'!");
            }

            using var s3Client = new AmazonS3Client();
            using var ms = new MemoryStream();

            await stream.CopyToAsync(ms);

            var utility = new TransferUtility(s3Client);
            var request = new TransferUtilityUploadRequest
            {
                BucketName = _bucketName,
                Key = userImage.ResourceKey,
                InputStream = ms,
                AutoCloseStream = true,
                AutoResetStreamPosition = true
            };

            await utility.UploadAsync(request);

            userImage.HasContent = true;

            await _dbContext.SaveChangesAsync();

            return CreateModel(userImage);
        }

        public async Task DeleteUserImage(int userImageId)
        {
            var userImage = await _dbContext.UserImages.FirstOrDefaultAsync(x => x.Id == userImageId);

            if (userImage == null)
            {
                throw new NotFoundException($"Could not find user image with id '{userImageId}'!");
            }

            _dbContext.UserImages.Remove(userImage);

            using var s3Client = new AmazonS3Client();

            await s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = userImage.ResourceKey
            });
        }

        public async Task DeleteUserImageOfUser(int userId)
        {
            var userImages = await _dbContext.UserImages
                .Where(x => x.UserId == userId)
                .ToListAsync();

            foreach (var userImage in userImages)
            {
                _dbContext.UserImages.Remove(userImage);

                await _dbContext.SaveChangesAsync();
                using var s3Client = new AmazonS3Client();

                await s3Client.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = userImage.ResourceKey
                });
            }
        }
    }
}
