using Amazon.DynamoDBv2.DataModel;

namespace Mcce22.SmartOffice.Users.Entities
{
    [DynamoDBTable("mcce22-smart-office-users")]
    public class User
    {
        [DynamoDBHashKey()]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string UserName { get; set; }

        [DynamoDBProperty]
        public string FirstName { get; set; }

        [DynamoDBProperty]
        public string LastName { get; set; }

        [DynamoDBProperty]
        public string Email { get; set; }
    }
}
