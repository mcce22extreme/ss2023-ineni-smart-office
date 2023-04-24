using Amazon.DynamoDBv2;
using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Core;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.Bookings
{
    public class Bootstrap : BootstrapBase
    {
        protected override string ApiPrefix => "bookingapi";

        protected override async Task ConfigureBuilder(WebApplicationBuilder builder)
        {
            await base.ConfigureBuilder(builder);

            var appSettings = Configuration.Get<AppSettings>();

            Log.Debug("Application Configuration:");
            Log.Debug(JsonConvert.SerializeObject(appSettings, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));

            builder.Services.AddAutoMapper(typeof(Bootstrap).Assembly);

#if DEBUG
            // Configure urls (only for local debugging)
            builder.WebHost.UseUrls(appSettings.BaseAddress);
#endif
            builder.Services.AddScoped<IAmazonDynamoDB>(s => new AmazonDynamoDBClient());

            builder.Services.AddScoped<IBookingManager, BookingManager>();
        }
    }
}
