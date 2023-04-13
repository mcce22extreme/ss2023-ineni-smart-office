using System.ComponentModel.DataAnnotations;
using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Workspaces.Entities
{
    public class WorkspaceConfiguration : EntityBase
    {
        [Required]
        public long DeskHeight { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int WorkspaceId { get; set; }
    }
}
