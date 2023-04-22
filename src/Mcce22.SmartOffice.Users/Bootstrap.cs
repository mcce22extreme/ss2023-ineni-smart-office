using Amazon.DynamoDBv2;
using Amazon.S3;
using Mcce22.SmartOffice.Core;
using Mcce22.SmartOffice.Users.Managers;
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

            builder.Services.AddSingleton<IAppSettings>(s => appSettings);

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

            builder.Services.AddSingleton(appSettings);

            builder.Services.AddScoped<IAmazonDynamoDB>(s => new AmazonDynamoDBClient());

            builder.Services.AddScoped<IAmazonS3>(s => new AmazonS3Client());

            builder.Services.AddScoped<IUserManager, UserManager>();

            builder.Services.AddScoped<IUserImageManager, UserImageManager>();
        }
    }
}
