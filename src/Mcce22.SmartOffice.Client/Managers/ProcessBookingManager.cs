using System.Net.Http;
using System.Threading.Tasks;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IProcessBookingManager
    {
        Task ProcessBookings();
    }

    public class ProcessBookingManager : IProcessBookingManager
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private readonly string _baseUrl;

        public ProcessBookingManager(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task ProcessBookings()
        {
            var respones = await HttpClient.GetStringAsync($"{_baseUrl}");
        }
    }
}
