using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IUserImageManager
    {
        Task<UserImageModel[]> GetList();

        Task Save(string userId, string filePath);

        Task Delete(string userImageId);
    }

    public class UserImageManager : ManagerBase<UserImageModel>, IUserImageManager
    {
        public UserImageManager(string baseUrl)
             : base($"{baseUrl}/user")
        {
        }

        public async Task Save(string userId, string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

            form.Add(fileContent, "formFile", Path.GetFileName(filePath));

            await HttpClient.PostAsync($"{BaseUrl}/{userId}/image", form);
        }

        //public async Task StoreContent(string userImageId, Stream stream)
        //{
        //    await HttpClient.PostAsync($"{BaseUrl}/{userImageId}/content", new StreamContent(stream));
        //}
    }
}
