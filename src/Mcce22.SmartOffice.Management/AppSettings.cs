using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Newtonsoft.Json;
using Serilog;
using System.Reflection;

namespace Mcce22.SmartOffice.Management
{
    public class AppSettings
    {
        static AppSettings()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true)
                .Build();

            Current = Config.Get<AppSettings>();
        }

        public static AppSettings Current { get; }

        public static IConfigurationRoot Config { get; }

        public string BaseAddress { get; set; }

        public string ConnectionString { get; set; }

        public SmptConfiguration SmptConfiguration { get; set; } = new SmptConfiguration();

        public StorageConfiguration StorageConfiguration { get; set; } = new StorageConfiguration();

        public async Task LoadConfigFromAWSSecretsManager()
        {
            using var client = new AmazonSecretsManagerClient();

            var request = new GetSecretValueRequest
            {
                SecretId = "mcce22-smart-office-management"
            };

            try
            {
                var response = await client.GetSecretValueAsync(request);

                var settings = JsonConvert.DeserializeObject<AppSettings>(response.SecretString);

                ConnectionString = settings.ConnectionString;
                SmptConfiguration = settings.SmptConfiguration;
                StorageConfiguration = settings.StorageConfiguration;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }
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
