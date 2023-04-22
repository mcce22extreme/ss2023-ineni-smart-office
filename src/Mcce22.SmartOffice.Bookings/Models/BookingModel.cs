namespace Mcce22.SmartOffice.Bookings.Models
{
    public class BookingModel
    {
        public string Id { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string UserId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string WorkspaceId { get; set; }

        public string WorkspaceNumber { get; set; }

        public string RoomNumber { get; set; }

        public bool Activated { get; set; }

        public bool InvitationSent { get; set; }
    }
}
