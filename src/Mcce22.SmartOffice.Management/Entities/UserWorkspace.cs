using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class UserWorkspace : EntityBase
    {
        public int UserId { get; set; }

        public int WorkspaceId { get; set; }

        public long DeskHeight { get; set; }

        public string SlideshowResourceKey { get; set; }

        public User User { get; }

        public Workspace Workspace { get; }
    }
}
