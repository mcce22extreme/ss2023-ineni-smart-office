using Amazon.DynamoDBv2;
using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Core;

namespace Mcce22.SmartOffice.Bookings
{
    public class Bootstrap : BootstrapBase
    {
        protected override string ApiPrefix => "bookingapi";

        protected override async Task ConfigureBuilder(WebApplicationBuilder builder)
        {
            await base.ConfigureBuilder(builder);

            builder.Services.AddAutoMapper(typeof(Bootstrap).Assembly);

            builder.Services.AddScoped<IAmazonDynamoDB>(s => new AmazonDynamoDBClient());

            builder.Services.AddScoped<IBookingManager, BookingManager>();
        }
    }
}
