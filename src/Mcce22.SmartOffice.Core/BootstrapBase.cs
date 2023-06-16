using System.Reflection;
using FluentValidation;
using Mcce22.SmartOffice.Core.Attributes;
using Mcce22.SmartOffice.Core.Common;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Core.Handlers;
using Mcce22.SmartOffice.Core.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Mcce22.SmartOffice.Core
{
    public abstract class BootstrapBase
    {
#if DEBUG
        private readonly string _baseAddress;
#endif

        protected IConfiguration Configuration { get; }

        protected BootstrapBase()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true)
                .Build();

#if DEBUG
            _baseAddress = Configuration.GetSection("BaseAddress")?.Value;
#endif
        }

        public async Task Run(string[] args)
        {
            // Configure serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            await ConfigureBuilder(builder);

            var app = builder.Build();

            await ConfigureApp(app);

            app.Run();
        }

        protected virtual Task ConfigureBuilder(WebApplicationBuilder builder)
        {
            var assembly = Assembly.GetEntryAssembly();
            var appInfo = new AppInfo(assembly.GetName());

            Log.Information($"Starting {appInfo.AppName} v{appInfo.AppVersion}...");

#if DEBUG
            // Configure urls (only for local debugging)
            builder.WebHost.UseUrls(_baseAddress);
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

            builder.Services.AddSingleton<IAppInfo>(appInfo);

            builder.Services.AddSingleton<IIdGenerator, IdGenerator>();

            // Configure dependency injection
            builder.Services.AddSingleton<IAppInfo>(appInfo);

            builder.Services.AddSingleton<IValidationProvider, ValidationProvider>();

            var validators = assembly
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableFrom(typeof(IValidator)))
                .ToList();

            foreach (var validator in validators)
            {
                builder.Services.Add(new ServiceDescriptor(typeof(IValidator), validator, ServiceLifetime.Singleton));
            }

            return Task.CompletedTask;
        }

        protected virtual Task ConfigureApp(WebApplication app)
        {
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.ConfigureExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            return Task.CompletedTask;
        }
    }
}
