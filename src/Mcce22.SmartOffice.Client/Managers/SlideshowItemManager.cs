using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface ISlideshowItemManager
    {
        Task<SlideshowItemModel[]> GetList();

        Task<SlideshowItemModel> Save(SlideshowItemModel user);

        Task StoreContent(int slideshowItemId, Stream stream);

        Task Delete(int slideshowItemId);
    }

    public class SlideshowItemManager : ManagerBase<SlideshowItemModel>, ISlideshowItemManager
    {
        public SlideshowItemManager(string baseUrl)
             : base($"{baseUrl}/slideshowitem")
        {
        }

        public async Task StoreContent(int slideshowItemId, Stream stream)
        {
            await HttpClient.PostAsync($"{BaseUrl}/{slideshowItemId}/content", new StreamContent(stream));
        }
    }
}
