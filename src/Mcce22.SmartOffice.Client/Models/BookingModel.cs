using System;

namespace Mcce22.SmartOffice.Client.Models
{
    public class BookingModel : IModel
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int WorkspaceId { get; set; }

        public int UserId { get; set; }

        public string RoomNumber { get; set; }

        public string WorkspaceNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string FullUserName { get { return $"{FirstName} {LastName} ({UserName})"; } }

        public bool Activated { get; set; }

        public bool InvitationSent { get; set; }
    }
}
