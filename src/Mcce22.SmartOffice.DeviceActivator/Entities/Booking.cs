using Amazon.DynamoDBv2.DataModel;
using Mcce22.SmartOffice.Core.Converters;

namespace Mcce22.SmartOffice.DeviceActivator.Entities
{
    [DynamoDBTable("mcce22-smart-office-bookings")]
    public class Booking
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty(converter: typeof(DateOnlyConverter))]
        public DateOnly StartDate { get; set; }

        [DynamoDBProperty(converter: typeof(TimeOnlyConverter))]
        public TimeOnly StartTime { get; set; }

        [DynamoDBProperty(converter: typeof(DateOnlyConverter))]
        public DateOnly EndDate { get; set; }

        [DynamoDBProperty(converter: typeof(TimeOnlyConverter))]
        public TimeOnly EndTime { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string FirstName { get; set; }

        [DynamoDBProperty]
        public string LastName { get; set; }

        [DynamoDBProperty]
        public string UserName { get; set; }

        [DynamoDBProperty]
        public string Email { get; set; }

        [DynamoDBProperty]
        public string WorkspaceId { get; set; }

        [DynamoDBProperty]
        public string WorkspaceNumber { get; set; }

        [DynamoDBProperty]
        public string RoomNumber { get; set; }

        [DynamoDBProperty]
        public bool Activated { get; set; }

        [DynamoDBProperty]
        public bool InvitationSent { get; set; }

        [DynamoDBProperty]
        public string ActivationCode { get; set; }
    }
}
