using Amazon.DynamoDBv2.DataModel;

namespace Mcce22.SmartOffice.DataIngress.Entities
{
    [DynamoDBTable("mcce22-smart-office-workspace-data")]
    public class WorkspaceData
    {
        [DynamoDBHashKey]
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
        public double Temperature { get; set; }

        [DynamoDBProperty]
        public double Humidity { get; set; }

        [DynamoDBProperty]
        public double Co2Level { get; set; }

        [DynamoDBProperty]
        public int Wei { get; set; }
    }
}
