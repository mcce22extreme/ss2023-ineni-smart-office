using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IUserImageManager
    {
        Task<UserImageModel[]> GetList();

        Task<UserImageModel> Save(UserImageModel user);

        Task StoreContent(int userImageId, Stream stream);

        Task Delete(int userImageId);
    }

    public class UserImageManager : ManagerBase<UserImageModel>, IUserImageManager
    {
        public UserImageManager(string baseUrl)
             : base($"{baseUrl}/userimage")
        {
        }

        public async Task StoreContent(int userImageId, Stream stream)
        {
            await HttpClient.PostAsync($"{BaseUrl}/{userImageId}/content", new StreamContent(stream));
        }
    }
}
