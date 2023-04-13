using System.ComponentModel.DataAnnotations;
using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Workspaces.Entities
{
    public class Workspace : EntityBase
    {
        [Required]
        public string WorkspaceNumber { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
