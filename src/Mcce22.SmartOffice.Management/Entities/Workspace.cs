using System.ComponentModel.DataAnnotations;
using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class Workspace : EntityBase
    {
        [Required]
        public string WorkspaceNumber { get; set; }

        public string RoomNumber { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public List<UserWorkspace> UserWorkspaces { get; }

        public List<Booking> Bookings { get; set; }
    }
}
