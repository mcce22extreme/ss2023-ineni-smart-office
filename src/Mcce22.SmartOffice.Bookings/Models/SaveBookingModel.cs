using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Bookings.Models
{
    public class SaveBookingModel
    {
        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string WorkspaceId { get; set; }
    }
}
