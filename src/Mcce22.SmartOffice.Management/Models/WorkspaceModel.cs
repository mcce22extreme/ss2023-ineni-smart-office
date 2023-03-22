namespace Mcce22.SmartOffice.Management.Models
{
    public class WorkspaceModel
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public string Number { get; set; }
    }

    public class SaveWorkspaceModel
    {
        public int RoomId { get; set; }

        public string Number { get; set; }
    }
}
