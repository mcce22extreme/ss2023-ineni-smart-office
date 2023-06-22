using Amazon.DynamoDBv2.DataModel;

namespace Mcce22.SmartOffice.DataIngress.Entities
{
    [DynamoDBTable("mcce22-smart-office-workspaces")]
    public class Workspace
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string WorkspaceNumber { get; set; }

        [DynamoDBProperty]
        public string RoomNumber { get; set; }

        [DynamoDBProperty]
        public int Wei { get; set; }
    }
}
