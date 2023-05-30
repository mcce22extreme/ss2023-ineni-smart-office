using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Core;
using Mcce22.SmartOffice.Workspaces.Managers;

namespace Mcce22.SmartOffice.Workspaces
{
    public class Bootstrap : BootstrapBase
    {
        protected override async Task ConfigureBuilder(WebApplicationBuilder builder)
        {
            await base.ConfigureBuilder(builder);

            builder.Services.AddAutoMapper(typeof(Bootstrap).Assembly);

            builder.Services.AddScoped<IDynamoDBContext>(s => new DynamoDBContext(new AmazonDynamoDBClient()));

            builder.Services.AddScoped<IWorkspaceManager, WorkspaceManager>();

            builder.Services.AddScoped<IWorkspaceConfigurationManager, WorkspaceConfigurationManager>();

            builder.Services.AddScoped<IWorkspaceDataManager, WorkspaceDataManager>();
        }
    }
}
