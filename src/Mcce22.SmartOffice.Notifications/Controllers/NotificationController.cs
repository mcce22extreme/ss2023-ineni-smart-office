using Mcce22.SmartOffice.Notifications.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Notifications.Controllers
{
    [ApiController]
    [Route("notify")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationManager _notificationManager;

        public NotificationController(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        [HttpPost]
        public async Task<IActionResult> Notify()
        {
            var count = await _notificationManager.ProcessPendingBookings();

            return Ok($"Successfully processed {count} bookings!");
        }
    }
}
