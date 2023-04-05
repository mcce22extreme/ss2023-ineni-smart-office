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

        public string StorageBaseAddress { get; set; }

        public string ConnectionString { get; set; }

        public SmptConfiguration SmptConfiguration { get; set; } = new SmptConfiguration();

        public StorageConfiguration StorageConfiguration { get; set; } = new StorageConfiguration();
    }

    public class SmptConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string SenderName { get; set; }
    }

    public class StorageConfiguration
    {
        public string BucketName { get; set; }

        public string BaseUrl { get; set; }
    }
}
