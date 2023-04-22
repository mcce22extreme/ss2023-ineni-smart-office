using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Managers
{
    public interface IUserImageManager
    {
        Task<UserImageModel[]> GetUserImages(string userId);

        Task<UserImageModel> StoreUserImage(string userId, IFormFile file);

        Task DeleteAllUserImage(string userId);

        Task DeleteUserImage(string userId, string userImageId);                
    }

    public class UserImageManager : IUserImageManager
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public UserImageManager(IAmazonS3 s3Client, IAppSettings appSettings)
        {
            _s3Client = s3Client;
            _bucketName = appSettings.BucketName;
        }

        public async Task<UserImageModel[]> GetUserImages(string userId)
        {
            var items = await _s3Client.ListObjectsAsync(_bucketName, userId);

            return items.S3Objects
                .Where(x => x.Size > 0)
                .Select(x => new UserImageModel
                {
                    Id = x.Key,
                    ResourceUrl = CreateResourceKey(_bucketName, userId, x.Key),
                    Size = x.Size
                })
                .ToArray();
        }

        public async Task<UserImageModel> StoreUserImage(string userId, IFormFile file)
        {
            using var ms = new MemoryStream();

            await file.CopyToAsync(ms);

            var key = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var utility = new TransferUtility(_s3Client);
            var request = new TransferUtilityUploadRequest
            {
                BucketName = _bucketName,
                Key = $"{userId}/{key}",
                InputStream = ms,
                AutoCloseStream = true,
                AutoResetStreamPosition = true
            };

            await utility.UploadAsync(request);

            return new UserImageModel
            {
                Id = key,
                ResourceUrl = CreateResourceKey(_bucketName, userId, key),
                Size = file.Length
            };
        }

        private string CreateResourceKey(string bucketName, string userId, string objectKey)
        {
            return $"https://{bucketName}.s3.amazonaws.com/{userId}/{objectKey}";
        }

        public async Task DeleteUserImage(string userId, string userImageId)
        {
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{userId}/{userImageId}"
            });
        }

        public async Task DeleteAllUserImage(string userId)
        {
            var items = await _s3Client.ListObjectsAsync(_bucketName, userId);

            foreach(var item in items.S3Objects)
            {
                await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = item.Key
                });
            }            
        }
    }
}
