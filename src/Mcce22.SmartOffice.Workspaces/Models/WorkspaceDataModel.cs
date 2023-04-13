﻿namespace Mcce22.SmartOffice.Workspaces.Models
{
    public class WorkspaceDataModel
    {
        public int Id { get; set; }

        public int WorkspaceId { get; set; }

        public string WorkspaceNumber { get; set; }

        public string RoomNumber { get; set; }

        public DateTime Timestamp { get; set; }

        public int Temperature { get; set; }

        public int Noise { get; set; }

        public int Humidity { get; set; }

        public int Co2 { get; set; }

        public int Luminosity { get; set; }
    }
}