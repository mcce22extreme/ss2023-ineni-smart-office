namespace Mcce22.SmartOffice.Management.Models
{
    public class RoomModel
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int NumberOfWorkspaces { get; set; }
    }

    public class SaveRoomModel
    {
        public string Number { get; set; }
    }
}
