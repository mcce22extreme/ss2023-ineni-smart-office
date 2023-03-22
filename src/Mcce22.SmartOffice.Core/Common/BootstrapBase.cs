using System.Reflection;
using FluentValidation;
using Mcce22.SmartOffice.Core.Attributes;
using Mcce22.SmartOffice.Core.Handlers;
using Mcce22.SmartOffice.Core.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.Core.Common
{
    public abstract class BootstrapBase
    {
        private static WebApplication _application;

        public BootstrapBase()
        {
            // Configure serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(AppSettings.Config)
                .CreateLogger();
        }

        public Task Run(params string[] args)
        {
            Log.Information($"Starting {AppInfo.Current.AppName} v{AppInfo.Current.AppVersion}...");
            Log.Information("Application Configuration:");
            Log.Information(JsonConvert.SerializeObject(AppSettings.Current, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));

            // Create application
            _application = CreateApplication(args);

            // Start application
            return _application.RunAsync();
        }

        protected virtual WebApplicationBuilder CreateWebApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure urls
            builder.WebHost.UseUrls(AppSettings.Current.BaseAddress);

            // Configure logging
            builder.Services.AddLogging(cfg =>
            {
                cfg.ClearProviders();
                cfg.AddSerilog(Log.Logger);
            });

            builder.Services.AddHttpContextAccessor();

            // Configure global filters
            builder.Services.AddControllersWithViews(opt =>
            {
                opt.Filters.Add(new TypeFilterAttribute(typeof(OperationLoggerAttribute)));
                opt.Filters.Add(new TypeFilterAttribute(typeof(OperationValidatorAttribute)));
            });

            // Configure api versioning
            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = ApiVersion.Default;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddEndpointsApiExplorer();

            // Configure swagger doc generation
            builder.Services.AddSwaggerGen(c =>
            {
                // Add xml comments from core assembly if available
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{typeof(BootstrapBase).Assembly.GetName().Name}.xml");
                if(File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }

                // Add xml comments from executing assembly if available
                filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly().GetName().Name}.xml");
                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.ConfigureOptions<SwaggerConfigOptions>();

            if (!string.IsNullOrEmpty(AppSettings.Current.AuthEndpoint))
            {
                builder.Services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = AppSettings.Current.AuthEndpoint;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };
                    });
            }

            // Configure dependecy injection
            builder.Services.AddAutoMapper(typeof(BootstrapBase).Assembly, Assembly.GetEntryAssembly());

            builder.Services.AddSingleton<IValidationProvider, ValidationProvider>();

            var validators = Assembly
                .GetEntryAssembly()
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableFrom(typeof(IValidator)))
                .ToList();

            foreach (var validator in validators)
            {
                builder.Services.Add(new ServiceDescriptor(typeof(IValidator), validator, ServiceLifetime.Singleton));
            }

            return builder;
        }

        protected virtual WebApplication CreateApplication(string[] args)
        {
            var builder = CreateWebApplicationBuilder(args);

            var app = builder.Build();

            app.ConfigureExceptionHandler();

            app.UseSwagger(opt =>
            {
                //if (!string.IsNullOrEmpty(_appSettings.ApiGatewayKey))
                //{
                //    opt.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //    {
                //        foreach (var path in swaggerDoc.Paths.ToList())
                //        {
                //            swaggerDoc.Paths.Add($"/{_appSettings.ApiGatewayKey}{path.Key}", path.Value);
                //            swaggerDoc.Paths.Remove(path.Key);
                //        }
                //    });
                //}
            });

            app.UseSwaggerUI(opt =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    opt.SwaggerEndpoint($"{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
                }
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            return app;
        }
    }
}
