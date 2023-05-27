using Mcce22.SmartOffice.Bookings.Models;

namespace Mcce22.SmartOffice.Bookings.Managers
{
    public interface IBookingManager
    {
        Task<BookingModel[]> GetBookings();

        Task<BookingModel> GetBooking(string bookingId);

        Task<BookingModel> CreateBooking(SaveBookingModel model);

        Task DeleteBooking(string bookingId);
    }
}
