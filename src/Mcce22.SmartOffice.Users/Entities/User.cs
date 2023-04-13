using System.ComponentModel.DataAnnotations;
using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Users.Entities
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
    }
}
