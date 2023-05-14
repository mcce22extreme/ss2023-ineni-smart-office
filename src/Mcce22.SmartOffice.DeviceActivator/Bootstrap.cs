using Amazon.DynamoDBv2;
using Amazon.IotData;
using Mcce22.SmartOffice.Core;
using Mcce22.SmartOffice.Core.Providers;
using Mcce22.SmartOffice.DeviceActivator.Managers;

namespace Mcce22.SmartOffice.DeviceActivator
{
    public class Bootstrap : BootstrapBase
    {
        protected override string ApiPrefix => "activatorapi";

        protected override async Task ConfigureBuilder(WebApplicationBuilder builder)
        {
            await base.ConfigureBuilder(builder);

            builder.Services.AddScoped<IAppConfigProvider, AppConfigProvider>();

            builder.Services.AddScoped<IAmazonDynamoDB>(s => new AmazonDynamoDBClient());

            builder.Services.AddScoped<IAmazonIotData>(s =>
            {
                var appConfigProvider = s.GetRequiredService<IAppConfigProvider>();
                var appConfig = appConfigProvider.GetAppConfig().Result;

                return new AmazonIotDataClient(appConfig.IoTDataEndpointAddress);
            });

            builder.Services.AddScoped<IDeviceManager, DeviceManager>();
        }
    }
}
