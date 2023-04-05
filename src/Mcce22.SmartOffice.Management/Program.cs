using System.Reflection;
using FluentValidation;
using Mcce22.SmartOffice.Core.Attributes;
using Mcce22.SmartOffice.Core.Common;
using Mcce22.SmartOffice.Core.Handlers;
using Mcce22.SmartOffice.Core.Providers;
using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.Management
{
    public class Program
    {
        private const string CORS_POLICY = "CORS_POLICY";

        public static async Task Main(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            var assemblyName = assembly.GetName();
            var appInfo = new AppInfo(assemblyName.Name, assemblyName.Version.ToString());

            await AppSettings.Current.LoadConfigFromAWSSecretsManager();

            // Configure serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(AppSettings.Config)
                .CreateLogger();

            Log.Information($"Starting {appInfo.AppName} v{appInfo.AppVersion}...");
            Log.Information("Application Configuration:");
            Log.Information(JsonConvert.SerializeObject(AppSettings.Current, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));

            var builder = WebApplication.CreateBuilder(args);

#if DEBUG
            // Configure urls (only for local debugging)
            builder.WebHost.UseUrls(AppSettings.Current.BaseAddress);
#endif

            builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

            // Configure logging
            builder.Services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddSerilog(Log.Logger);
            });

            // Configure global filters
            builder.Services.AddControllersWithViews(opt =>
            {
                opt.Filters.Add(new TypeFilterAttribute(typeof(OperationLoggerAttribute)));
                opt.Filters.Add(new TypeFilterAttribute(typeof(OperationValidatorAttribute)));
            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            // Configure dependency injection
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(AppSettings.Current.ConnectionString));
            //builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("smartofficedb"));

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddScoped<IWorkspaceManager, WorkspaceManager>();

            builder.Services.AddScoped<IUserManager, UserManager>();

            builder.Services.AddScoped<IUserWorkspaceManager, UserWorkspaceManager>();

            builder.Services.AddScoped<IBookingManager, BookingManager>();

            builder.Services.AddSingleton<StorageConfiguration>(AppSettings.Current.StorageConfiguration);

            builder.Services.AddScoped<ISlideshowItemManager, SlideshowItemManager>();

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddSingleton<IValidationProvider, ValidationProvider>();

            builder.Services.AddSingleton<IAppInfo>(appInfo);

            var validators = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableFrom(typeof(IValidator)))
                .ToList();

            foreach (var validator in validators)
            {
                builder.Services.Add(new ServiceDescriptor(typeof(IValidator), validator, ServiceLifetime.Singleton));
            }

            var app = builder.Build();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors(CORS_POLICY);

            app.ConfigureExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            // Perform database migration
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Check if we can migrate database automatically
            if (dbContext.Database.IsRelational())
            {
                dbContext.Database.Migrate();
            }

            app.Run();
        }
    }
}
