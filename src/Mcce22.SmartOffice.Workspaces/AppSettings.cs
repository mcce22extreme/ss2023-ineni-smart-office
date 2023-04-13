using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.Workspaces
{
    public class AppSettings
    {
        public string BaseAddress { get; set; }

        public string ConnectionString { get; set; }

        public string SecretId { get; set; }

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
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }
        }
    }
}
