using Amazon.DynamoDBv2.DataModel;

namespace Mcce22.SmartOffice.Workspaces.Entities
{
    [DynamoDBTable("mcce22-smart-office-workspace-data")]
    public class WorkspaceData
    {
        [DynamoDBHashKey()]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string WorkspaceId { get; set; }

        [DynamoDBProperty]
        public string WorkspaceNumber { get; set; }

        [DynamoDBProperty]
        public string RoomNumber { get; set; }

        [DynamoDBProperty]
        public DateTime Timestamp { get; set; }

        [DynamoDBProperty]
        public int Temperature { get; set; }

        [DynamoDBProperty]
        public int Noise { get; set; }

        [DynamoDBProperty]
        public int Humidity { get; set; }

        [DynamoDBProperty]
        public int Co2 { get; set; }

        [DynamoDBProperty]
        public int Luminosity { get; set; }
    }
}
