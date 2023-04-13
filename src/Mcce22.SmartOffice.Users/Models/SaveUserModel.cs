using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Users.Models
{
    public class SaveUserModel
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
