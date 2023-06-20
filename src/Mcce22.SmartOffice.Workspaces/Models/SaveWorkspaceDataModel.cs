using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Workspaces.Models
{
    public class SaveWorkspaceDataModel
    {
        [Required]
        public string WorkspaceId { get; set; }

        public DateTime? Timestamp { get; set; }

        public int Temperature { get; set; }

        public int NoiseLevel { get; set; }

        public int Co2Level { get; set; }
    }
}
