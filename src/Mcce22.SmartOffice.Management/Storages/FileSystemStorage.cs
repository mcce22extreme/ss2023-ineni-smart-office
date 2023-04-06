namespace Mcce22.SmartOffice.Management.Storages
{
    public class FileSystemStorage : IFileStorage
    {
        private readonly string _basePath;

        public FileSystemStorage(string basePath)
        {
            _basePath = basePath;
        }

        public async Task<string> StoreFile(Stream stream)
        {
            var resourceKey = Guid.NewGuid().ToString();
            var filePath = Path.Combine(_basePath, resourceKey);

            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            await stream.CopyToAsync(fs);

            return resourceKey;
        }

        public Task<Stream> GetFileContent(string resourceKey)
        {
            var filePath = Path.Combine(_basePath, resourceKey);

            return Task.FromResult<Stream>(new FileStream(filePath, FileMode.Open, FileAccess.Read));
        }

        public Task DeleteFile(string resourceKey)
        {
            var filePath = Path.Combine(_basePath, resourceKey);

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }
}
