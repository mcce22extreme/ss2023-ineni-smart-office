using System.Diagnostics;
using System.Net;
using System.Reflection;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.SimpleSystemsManagement;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Core.Providers;
using Mcce22.SmartOffice.Notifications.Managers;
using Mcce22.SmartOffice.Notifications.Services;
using Microsoft.Extensions.Configuration;
using Serilog;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Mcce22.SmartOffice.Notifications;
public class Functions
{
    public Functions()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Configure serilog
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(config)
            .CreateLogger();
    }

    public APIGatewayProxyResponse HandleRequest(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information($"[In ] {nameof(HandleRequest)}");

        var appConfigProvider = new AppConfigProvider(new AmazonSimpleSystemsManagementClient());
        var emailService = new EmailService(appConfigProvider);

        var notificationManager = new NotificationManager(emailService, new IdGenerator());

        var count = notificationManager.ProcessPendingBookings().Result;

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = $"Successfully processed {count} bookings!",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        Log.Information($"[Out ] {nameof(HandleRequest)} - {stopwatch.Elapsed}");

        return response;
    }
}
