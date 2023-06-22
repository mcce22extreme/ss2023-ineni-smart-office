namespace Mcce22.SmartOffice.DeviceActivator.Models
{
    public class ActivateModel
    {
        public string WorkspaceNumber { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BookingId { get; set; }

        public long DeskHeight { get; set; }

        public string[] UserImageUrls { get; set; }
    }
}
