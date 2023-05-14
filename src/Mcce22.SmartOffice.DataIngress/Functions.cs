using System.Diagnostics;
using System.Net;
using System.Reflection;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.DataIngress.Managers;
using Microsoft.Extensions.Configuration;
using Serilog;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace Mcce22.SmartOffice.DataIngress;

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

    public APIGatewayProxyResponse HandleRequest(double temperature, ILambdaContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information($"[In ] {nameof(HandleRequest)}");

        Log.Debug($"Temperature:{temperature}");

        var workspaceDataManager = new WorkspaceDataManager(new AmazonDynamoDBClient(), new IdGenerator());

        Task.WaitAll(workspaceDataManager.CreateWorkspaceData(new Models.SaveWorkspaceDataModel
        {
            Temperature = temperature
        }));

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = $"Successfully created workspace data for ''!",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        Log.Information($"[Out ] {nameof(HandleRequest)} - {stopwatch.Elapsed}");

        return response;
    }
}
