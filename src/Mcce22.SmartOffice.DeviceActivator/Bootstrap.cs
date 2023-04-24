using Amazon.DynamoDBv2;
using Amazon.IotData;
using Mcce22.SmartOffice.Core;
using Mcce22.SmartOffice.DeviceActivator.Managers;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.DeviceActivator
{
    public class Bootstrap : BootstrapBase
    {
        protected override string ApiPrefix => "activatorapi";

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

#if DEBUG
            // Configure urls (only for local debugging)
            builder.WebHost.UseUrls(appSettings.BaseAddress);
#endif

            builder.Services.AddSingleton(appSettings);

            builder.Services.AddScoped<IAmazonDynamoDB>(s => new AmazonDynamoDBClient());

            builder.Services.AddScoped<IAmazonIotData>(s => new AmazonIotDataClient(appSettings.EndpointAddress));

            builder.Services.AddScoped<IDeviceManager, DeviceManager>();
        }
    }
}
