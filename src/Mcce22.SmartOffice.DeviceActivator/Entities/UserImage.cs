using Amazon.DynamoDBv2.DataModel;

namespace Mcce22.SmartOffice.DeviceActivator.Entities
{
    [DynamoDBTable("mcce22-smart-office-userimages")]
    public class UserImage
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string ResourceKey { get; set; }

        [DynamoDBProperty]
        public string Url { get; set; }
    }
}
