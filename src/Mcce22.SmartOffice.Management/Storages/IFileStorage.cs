namespace Mcce22.SmartOffice.Management.Storages
{
    public interface IFileStorage
    {
        Task<string> StoreFile(Stream stream);

        Task<Stream> GetFileContent(string resourceKey);

        Task DeleteFile(string resourceKey);
    }
}
