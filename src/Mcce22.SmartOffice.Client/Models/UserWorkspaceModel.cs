namespace Mcce22.SmartOffice.Client.Models
{
    public class UserWorkspaceModel : IModel
    {
        public int Id { get; set; }

        public long DeskHeight { get; set; }

        public string SlideshowResourceKey { get; set; }

        public int WorkspaceId { get; set; }

        public int UserId { get; set; }

        public string RoomNumber { get; set; }

        public string WorkspaceNumber { get; set; }
    }
}
