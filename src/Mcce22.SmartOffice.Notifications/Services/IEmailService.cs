using Mcce22.SmartOffice.Notifications.Entities;

namespace Mcce22.SmartOffice.Notifications.Services
{
    public interface IEmailService
    {
        Task SendMail(Booking booking);
    }
}
