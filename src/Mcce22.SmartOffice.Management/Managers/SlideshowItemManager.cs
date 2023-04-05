using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper;
using Mcce22.SmartOffice.Core.Common;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.Management.Entities;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management.Managers
{
    public interface ISlideshowItemManager
    {
        Task<SlideshowItemModel[]> GetSlideshowItems(int userId);

        Task<SlideshowItemModel> CreateSlideshowItem(SaveSlideshowItemModel model);

        Task<SlideshowItemModel> StoreSlideshowItemContent(int slideshowItemId, Stream stream);

        Task DeleteSlideshowItem(int slideshowItemId);
    }

    public class SlideshowItemManager : ISlideshowItemManager
    {
        private readonly string _baseUrl;
        private readonly string _bucketName;
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public SlideshowItemManager(StorageConfiguration storageConfiguration, AppDbContext dbContext, IMapper mapper)
        {
            _baseUrl = storageConfiguration.BaseUrl;
            _bucketName = storageConfiguration.BucketName;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private SlideshowItemModel CreateModel(SlideshowItem slideshowItem)
        {
            return new SlideshowItemModel
            {
                Id = slideshowItem.Id,
                ResourceUrl = $"{_baseUrl}/{slideshowItem.ResourceKey}",
                UserId = slideshowItem.User.Id
            };
        }

        public async Task<SlideshowItemModel[]> GetSlideshowItems(int userId)
        {
            var slideshowItemsQuery = _dbContext.SlideshowItems
                .Include(x => x.User)
                .AsQueryable();

            if (userId > 0)
            {
                slideshowItemsQuery = slideshowItemsQuery.Where(x => x.User.Id == userId);
            }

            var slideshowItems = await slideshowItemsQuery
                .ToListAsync();

            return slideshowItems.Select(CreateModel).ToArray();
        }

        public async Task<SlideshowItemModel> StoreSlideshowItemContent(int slideshowItemId, Stream stream)
        {
            var slideshowItem = await _dbContext.SlideshowItems
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == slideshowItemId);

            if (slideshowItem == null)
            {
                throw new NotFoundException($"Could not find item with id '{slideshowItemId}'!");
            }

            using var s3Client = new AmazonS3Client();
            using var ms = new MemoryStream();

            await stream.CopyToAsync(ms);

            var utility = new TransferUtility(s3Client);
            var request = new TransferUtilityUploadRequest
            {
                BucketName = _bucketName,
                Key = slideshowItem.ResourceKey,
                InputStream = ms,
                AutoCloseStream = true,
                AutoResetStreamPosition = true
            };

            await utility.UploadAsync(request);

            slideshowItem.HasContent = true;

            await _dbContext.SaveChangesAsync();

            return CreateModel(slideshowItem);
        }

        public async Task<SlideshowItemModel> CreateSlideshowItem(SaveSlideshowItemModel model)
        {
            var slideshowItem = new SlideshowItem
            {
                ResourceKey = $"{Guid.NewGuid()}{Path.GetExtension(model.FileName)}",
                User = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId)
            };

            await _dbContext.SlideshowItems.AddAsync(slideshowItem);
            await _dbContext.SaveChangesAsync();

            return CreateModel(slideshowItem);
        }

        public async Task DeleteSlideshowItem(int slideshowItemId)
        {
            var slideshowItem = await _dbContext.SlideshowItems.FirstOrDefaultAsync(x => x.Id == slideshowItemId);

            if (slideshowItem == null)
            {
                throw new NotFoundException($"Could not find item with id '{slideshowItemId}'!");
            }

            using var s3Client = new AmazonS3Client();

            await s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = slideshowItem.ResourceKey
            });

            _dbContext.SlideshowItems.Remove(slideshowItem);

            await _dbContext.SaveChangesAsync();
        }
    }
}
