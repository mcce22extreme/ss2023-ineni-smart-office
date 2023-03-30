using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Management.Models
{
    public class BookingModel
    {
        public int Id { get; set; }
        
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int UserId { get; set; }

        public int WorkspaceId { get; set; }

        public bool Activated { get; set; }

        public bool InvitationSent { get; set; }

        public string RoomNumber { get; set; }

        public string WorkspaceNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }
    }

    public class SaveBookingModel
    {
        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int WorkspaceId { get; set; }
    }

    public class CheckAvailabilityModel
    {
        public int WorkspaceId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public bool Available { get; set; }
    }
}
