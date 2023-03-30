using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Models;
using Mcce22.SmartOffice.Management.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController
    {
        private readonly IBookingManager _bookingManager;

        public BookingController(IBookingManager bookingManager)
        {
            _bookingManager = bookingManager;
        }

        public async Task<BookingModel[]> GetBookings([FromQuery] BookingQuery query)
        {
            return await _bookingManager.GetBookings(query);
        }

        [HttpGet("{bookingId}")]
        public async Task<BookingModel> GetBooking(int bookingId)
        {
            return await _bookingManager.GetBooking(bookingId);
        }

        [HttpPost]
        public async Task<BookingModel> CreateBooking([FromBody] SaveBookingModel model)
        {
            return await _bookingManager.CreateBooking(model);
        }

        [HttpDelete("{bookingId}")]
        public async Task DeleteBooking(int bookingId)
        {
            await _bookingManager.DeleteBooking(bookingId);
        }

        [HttpGet("checkavailability")]
        public async Task<CheckAvailabilityModel> CheckAvailability([FromQuery] CheckAvailabilityQuery query)
        {
            return await _bookingManager.CheckAvailability(query);
        }

        [HttpPost("activate/{bookingid}")]
        public async Task ActivateBooking(int bookingId)
        {
            await _bookingManager.ActivateBooking(bookingId);
        }

        [HttpPost("process")]
        public async Task ProcessBookings()
        {
            await _bookingManager.ProcessBookings();
        }
    }
}
