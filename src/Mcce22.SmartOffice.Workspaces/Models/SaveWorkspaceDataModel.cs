using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Workspaces.Models
{
    public class SaveWorkspaceDataModel
    {
        [Required]
        public string WorkspaceId { get; set; }

        public DateTime? Timestamp { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public double Co2Level { get; set; }
    }
}
