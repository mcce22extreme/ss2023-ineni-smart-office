using System.ComponentModel.DataAnnotations;
using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class UserWorkspace : EntityBase
    {
        public long DeskHeight { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Workspace Workspace { get; set; }
    }
}
