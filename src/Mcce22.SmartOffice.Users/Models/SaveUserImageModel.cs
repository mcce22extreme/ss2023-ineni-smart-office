using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Users.Models
{
    public class SaveUserImageModel
    {
        [Required]
        public string FileName { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
