using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;

namespace Mcce22.SmartOffice.Notifications
{
    public interface IAppSettings
    {
        string ActivatorEndpointAddress { get; }

        SmptConfiguration SmptConfiguration { get; }
    }

    public class AppSettings : IAppSettings
    {
        public string SecretName { get; set; }

        public string ActivatorEndpointAddress { get; set; }

        public SmptConfiguration SmptConfiguration { get; set; } = new SmptConfiguration();

        public async Task LoadConfigFromAWSSecretsManager()
        {
            if (!string.IsNullOrEmpty(SecretName))
            {
                using var client = new AmazonSecretsManagerClient();

                var request = new GetSecretValueRequest
                {
                    SecretId = SecretName
                };

                try
                {
                    var response = await client.GetSecretValueAsync(request);

                    var settings = JsonConvert.DeserializeObject<AppSettings>(response.SecretString);

                    ActivatorEndpointAddress = settings.ActivatorEndpointAddress;
                    SmptConfiguration = settings.SmptConfiguration;
                }
                catch (Exception ex)
                {
                }
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
}
