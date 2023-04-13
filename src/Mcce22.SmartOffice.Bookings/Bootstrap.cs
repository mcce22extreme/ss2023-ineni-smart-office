using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Bookings.Services;
using Mcce22.SmartOffice.Core;
using Microsoft.EntityFrameworkCore;
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
            await appSettings.LoadConfigFromAWSSecretsManager();

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
            //builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("bookingdb"));
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(appSettings.ConnectionString));

            builder.Services.AddSingleton(appSettings.SmptConfiguration);

            builder.Services.AddScoped<IBookingManager, BookingManager>();

            builder.Services.AddScoped<IEmailService, EmailService>();
        }

        protected override async Task ConfigureApp(WebApplication app)
        {
            await base.ConfigureApp(app);

            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Check if we can migrate database automatically
            if (dbContext.Database.IsRelational())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
