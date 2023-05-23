using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.SimpleSystemsManagement;
using Mcce22.SmartOffice.Core;
using Mcce22.SmartOffice.Users.Managers;

namespace Mcce22.SmartOffice.Users
{
    public class Bootstrap : BootstrapBase
    {
        protected override async Task ConfigureBuilder(WebApplicationBuilder builder)
        {
            await base.ConfigureBuilder(builder);

            builder.Services.AddAutoMapper(typeof(Bootstrap).Assembly);

            builder.Services.AddScoped<IDynamoDBContext>(s => new DynamoDBContext(new AmazonDynamoDBClient()));

            builder.Services.AddScoped<IAmazonS3>(s => new AmazonS3Client());

            builder.Services.AddScoped<IAmazonSimpleSystemsManagement>(s => new AmazonSimpleSystemsManagementClient());

            builder.Services.AddScoped<IUserManager, UserManager>();

            builder.Services.AddScoped<IUserImageManager, UserImageManager>();
        }
    }
}
