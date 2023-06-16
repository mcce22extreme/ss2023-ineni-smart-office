﻿namespace Mcce22.SmartOffice.Workspaces.Models
{
    public class WorkspaceDataModel
    {
        public string Id { get; set; }

        public string WorkspaceId { get; set; }

        public string WorkspaceNumber { get; set; }

        public string RoomNumber { get; set; }

        public DateTime Timestamp { get; set; }

        public int Temperature { get; set; }

        public int Noise { get; set; }

        public int Co2 { get; set; }

        public int Wei { get; set; }
    }
}
