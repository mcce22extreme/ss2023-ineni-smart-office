using System.ComponentModel.DataAnnotations;
using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Workspaces.Entities
{
    public class WorkspaceData : EntityBase
    {
        [Required]
        public int WorkspaceId { get; set; }

        [Required]
        public string WorkspaceNumber { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public int Temperature { get; set; }

        public int Noise { get; set; }

        public int Humidity { get; set; }

        public int Co2 { get; set; }

        public int Luminosity { get; set; }
    }
}
