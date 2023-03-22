using Mcce22.SmartOffice.Core.Common;
using Mcce22.SmartOffice.Management.Managers;
using Microsoft.EntityFrameworkCore;

namespace Mcce22.SmartOffice.Management.Common
{
    public class Bootstrap : BootstrapBase
    {
        protected override WebApplicationBuilder CreateWebApplicationBuilder(string[] args)
        {
            var builder = base.CreateWebApplicationBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("managementdb"));

            builder.Services.AddScoped<IRoomManager, RoomManager>();
            builder.Services.AddScoped<IWorkspaceManager, WorkspaceManager>();
            builder.Services.AddScoped<IUserManager, UserManager>();
            builder.Services.AddScoped<IUserWorkspaceManager, UserWorkspaceManager>();

            return builder;
        }
    }
}
