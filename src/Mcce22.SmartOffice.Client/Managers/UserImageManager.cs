using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HeyRed.Mime;
using Mcce22.SmartOffice.Client.Models;
using Newtonsoft.Json;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IUserImageManager
    {
        Task<UserImageModel[]> GetList(string userId);

        Task Save(string userId, string filePath);

        Task Delete(string userImageId);
    }

    public class UserImageManager : IUserImageManager
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private readonly string _baseUrl;

        public UserImageManager(string baseUrl)

        {
            _baseUrl = $"{baseUrl}/userimage/";
        }

        public async Task<UserImageModel[]> GetList(string userId)
        {
            var url = $"{_baseUrl}/{userId}";
            var json = await HttpClient.GetStringAsync(url);

            var entries = JsonConvert.DeserializeObject<UserImageModel[]>(json);

            return entries;
        }

        public async Task Delete(string userImageId)
        {
            var url = $"{_baseUrl}/{userImageId}";

            await HttpClient.DeleteAsync(url);
        }

        public async Task Save(string userId, string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var content = new MultipartFormDataContent
            {
                {
                    new StreamContent(stream)
                    {
                        Headers =
                {
                    ContentLength = stream.Length,
                    ContentType = new MediaTypeHeaderValue(MimeTypesMap.GetMimeType(filePath))
                }
                    },
                    "File",
                    Path.GetFileName(filePath)
                }
            };

            await HttpClient.PostAsync($"{_baseUrl}/{userId}", content);
        }
    }
}
