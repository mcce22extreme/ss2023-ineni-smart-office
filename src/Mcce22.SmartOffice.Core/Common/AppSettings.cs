using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Mcce22.SmartOffice.Core.Common
{
    public class AppSettings
    {
        static AppSettings()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddSecretsManager(
                    configurator: opt =>
                    {
                        opt.SecretFilter = e => e.Name == "mcce22-smart-office-management-db";
                        opt.KeyGenerator = (e, s) => "ConnectionString";
                    })
                .Build();            

            Current = Config.Get<AppSettings>();
        }

        public static AppSettings Current { get; }

        public static IConfigurationRoot Config { get; }

        public string BaseAddress { get; set; }

        public string ConnectionString { get; set; }
    }
}
