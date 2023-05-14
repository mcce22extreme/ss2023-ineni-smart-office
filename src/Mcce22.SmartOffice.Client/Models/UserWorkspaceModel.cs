namespace Mcce22.SmartOffice.Client.Models
{
    public class UserWorkspaceModel : IModel
    {
        public string Id { get; set; }

        public long DeskHeight { get; set; }

        public string SlideshowResourceKey { get; set; }

        public string WorkspaceId { get; set; }

        public string UserId { get; set; }

        public string RoomNumber { get; set; }

        public string WorkspaceNumber { get; set; }
    }
}
