using System.ComponentModel.DataAnnotations;
using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class Booking : EntityBase
    {
        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Workspace Workspace { get; set; }

        public bool Activated { get; set; }

        public bool InvitationSent { get; set; }
    }
}
