using System.Diagnostics;
using System.Net;
using System.Reflection;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Notifications.Managers;
using Mcce22.SmartOffice.Notifications.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Mcce22.SmartOffice.Notifications;
public class Functions
{
    private readonly AppSettings _appSettings;

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

        _appSettings = config.Get<AppSettings>();
    }

    public APIGatewayProxyResponse HandleRequest(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information("[In ]  Invoking Mcce22.SmartOffice.Notifications.HandleRequest...");

        Task.WaitAll(_appSettings.LoadConfigFromAWSSecretsManager());

        Log.Debug("Application Configuration: " + JsonConvert.SerializeObject(_appSettings, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        }));

        var emailService = new EmailService(_appSettings.ActivatorEndpointAddress, _appSettings.SmptConfiguration);

        var notificationManager = new NotificationManager(emailService, new IdGenerator());

        var count = notificationManager.ProcessPendingBookings().Result;

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = $"Successfully processed {count} bookings!",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        Log.Information($"[Out ] Invokation of Mcce22.SmartOffice.Notifications.HandleRequest took {stopwatch.Elapsed}.");

        return response;
    }
}
