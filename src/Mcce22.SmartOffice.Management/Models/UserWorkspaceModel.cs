using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Management.Models
{
    public class UserWorkspaceModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int WorkspaceId { get; set; }

        public long DeskHeight { get; set; }

        public string WorkspaceNumber { get; set; }

        public string RoomNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }
    }

    public class SaveUserWorkspaceModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int WorkspaceId { get; set; }

        public long DeskHeight { get; set; }

        public string SlideshowResourceKey { get; set; }
    }
}
