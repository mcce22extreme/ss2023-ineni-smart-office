using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SimpleSystemsManagement;
using Mcce22.SmartOffice.Core;
using Mcce22.SmartOffice.Core.Providers;
using Mcce22.SmartOffice.Notifications.Managers;
using Mcce22.SmartOffice.Notifications.Services;

namespace Mcce22.SmartOffice.Notifications
{
    public class Bootstrap : BootstrapBase
    {
        protected override async Task ConfigureBuilder(WebApplicationBuilder builder)
        {
            await base.ConfigureBuilder(builder);

            builder.Services.AddScoped<IDynamoDBContext>(s => new DynamoDBContext(new AmazonDynamoDBClient()));

            builder.Services.AddScoped<IAmazonSimpleSystemsManagement>(s => new AmazonSimpleSystemsManagementClient());

            builder.Services.AddScoped<IAppConfigProvider, AppConfigProvider>();

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<INotificationManager, NotificationManager>();
        }
    }
}
