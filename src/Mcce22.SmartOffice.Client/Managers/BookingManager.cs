using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IBookingManager
    {
        Task<BookingModel[]> GetList();

        Task<BookingModel> Save(BookingModel booking);

        Task Delete(string bookingId);

        Task ProcessBookings();
    }

    public class BookingManager : ManagerBase<BookingModel>, IBookingManager
    {
        public BookingManager(string baseUrl)
            : base($"{baseUrl}/booking")
        {
        }

        public async Task ProcessBookings()
        {
            await HttpClient.PostAsync($"{BaseUrl}/process", null);
        }
    }
}
