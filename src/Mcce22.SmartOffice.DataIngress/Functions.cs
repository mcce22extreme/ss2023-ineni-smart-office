using System.Diagnostics;
using System.Net;
using System.Reflection;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.DataIngress.Generators;
using Mcce22.SmartOffice.DataIngress.Managers;
using Mcce22.SmartOffice.DataIngress.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

    public APIGatewayProxyResponse HandleRequest(SaveWorkspaceDataModel model, ILambdaContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        Log.Information($"[In ] {nameof(HandleRequest)}");

        Log.Debug($"Ingress Data:{JsonConvert.SerializeObject(model)}");

        var workspaceDataManager = new WorkspaceDataManager(new AmazonDynamoDBClient(), new IdGenerator(), new WeiGenerator());

        Task.WaitAll(workspaceDataManager.CreateWorkspaceData(model));

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = $"Successfully created workspace data for ''!",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } },
        };

        Log.Information($"[Out ] {nameof(HandleRequest)} - {stopwatch.Elapsed}");

        return response;
    }
}
