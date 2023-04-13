using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Bookings.Entities
{
    public class Booking : EntityBase
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int WorkspaceId { get; set; }

        public string WorkspaceNumber { get; set; }

        public string RoomNumber { get; set; }

        public bool Activated { get; set; }

        public bool InvitationSent { get; set; }
    }
}
