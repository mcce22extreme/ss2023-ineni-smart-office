using Mcce22.SmartOffice.Core;
using Mcce22.SmartOffice.Users.Managers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.Users
{
    public class Bootstrap : BootstrapBase
    {
        protected override string ApiPrefix => "userapi";

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
            //builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("usersdb"));
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(appSettings.ConnectionString));

            builder.Services.AddSingleton(appSettings.StorageConfiguration);

            builder.Services.AddScoped<IUserManager, UserManager>();

            builder.Services.AddScoped<IUserImageManager, UserImageManager>();
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
