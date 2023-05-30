namespace Mcce22.SmartOffice.Notifications.Managers
{
    public interface INotificationManager
    {
        Task<int> ProcessPendingBookings();
    }
}
