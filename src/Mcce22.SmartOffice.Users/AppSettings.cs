using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.Users
{
    public class AppSettings
    {
        public string BaseAddress { get; set; }

        public string ConnectionString { get; set; }

        public string SecretId { get; set; }

        public StorageConfiguration StorageConfiguration { get; set; } = new StorageConfiguration();

        public async Task LoadConfigFromAWSSecretsManager()
        {
            if (!string.IsNullOrEmpty(SecretId))
            {
                using var client = new AmazonSecretsManagerClient();

                var request = new GetSecretValueRequest
                {
                    SecretId = SecretId
                };

                try
                {
                    var response = await client.GetSecretValueAsync(request);

                    var settings = JsonConvert.DeserializeObject<AppSettings>(response.SecretString);

                    ConnectionString = settings.ConnectionString;
                    StorageConfiguration = settings.StorageConfiguration;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }
        }
    }

    public class StorageConfiguration
    {
        public string BucketName { get; set; }

        public string BaseUrl { get; set; }
    }
}
