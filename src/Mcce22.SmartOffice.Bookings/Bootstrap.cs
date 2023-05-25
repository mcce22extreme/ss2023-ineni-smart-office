using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Core;

namespace Mcce22.SmartOffice.Bookings
{
    public class Bootstrap : BootstrapBase
    {
        protected override async Task ConfigureBuilder(WebApplicationBuilder builder)
        {
            await base.ConfigureBuilder(builder);

            builder.Services.AddAutoMapper(typeof(Bootstrap).Assembly);

            builder.Services.AddScoped<IDynamoDBContext>(s => new DynamoDBContext(new AmazonDynamoDBClient()));

            builder.Services.AddScoped<IBookingManager, BookingManager>();
        }
    }
}
