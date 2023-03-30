using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class Workspace : EntityBase
    {
        public int RoomId { get; set; }

        public string Number { get; set; }        

        public Room Room { get; }

        public List<UserWorkspace> UserWorkspaces { get; }

        public List<Booking> Bookings { get; set; }
    }
}
