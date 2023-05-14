using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Bookings.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Bookings.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController
    {
        private readonly IBookingManager _bookingManager;

        public BookingController(IBookingManager bookingManager)
        {
            _bookingManager = bookingManager;
        }

        [HttpGet]
        public async Task<BookingModel[]> GetBookings()
        {
            return await _bookingManager.GetBookings();
        }

        [HttpGet("{bookingId}")]
        public async Task<BookingModel> GetBooking(string bookingId)
        {
            return await _bookingManager.GetBooking(bookingId);
        }

        [HttpPost]
        public async Task<BookingModel> CreateBooking([FromBody] SaveBookingModel model)
        {
            return await _bookingManager.CreateBooking(model);
        }

        [HttpDelete("{bookingId}")]
        public async Task DeleteBooking(string bookingId)
        {
            await _bookingManager.DeleteBooking(bookingId);
        }

        //[HttpPost("activate/{bookingid}")]
        //public async Task<BookingModel> ActivateBooking(string bookingId)
        //{
        //    return await _bookingManager.ActivateBooking(bookingId);
        //}

        //[HttpPost("process")]
        //public async Task ProcessBookings()
        //{
        //    await _bookingManager.ProcessBookings();
        //}
    }
}
