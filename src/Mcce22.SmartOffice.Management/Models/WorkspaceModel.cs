using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Management.Models
{
    public class WorkspaceModel
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public string WorkspaceNumber { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }

    public class SaveWorkspaceModel
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
