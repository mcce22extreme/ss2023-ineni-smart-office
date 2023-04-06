using System.ComponentModel.DataAnnotations;
using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class User : EntityBase
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public List<UserWorkspace> UserWorkspaces { get; set; }

        public List<Booking> Bookings { get; set; }

        public List<SlideshowItem> SlideshowItems { get; set; }
    }
}
