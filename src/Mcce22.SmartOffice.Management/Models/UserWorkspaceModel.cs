namespace Mcce22.SmartOffice.Management.Models
{
    public class UserWorkspaceModel
    {
        public int UserId { get; set; }

        public int WorkspaceId { get; set; }

        public long DeskHeight { get; set; }

        public string SlideshowResourceKey { get; set; }
    }

    public class SaveUserWorkspaceModel
    {
        public int UserId { get; set; }

        public int WorkspaceId { get; set; }

        public long DeskHeight { get; set; }

        public string SlideshowResourceKey { get; set; }
    }
}
