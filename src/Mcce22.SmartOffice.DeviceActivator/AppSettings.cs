using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.DeviceActivator
{
    public interface IAppSettings
    {
        string BaseAddress { get; set; }

        string EndpointAddress { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public string SecretName { get; set; }

        public string BaseAddress { get; set; }

        public string EndpointAddress { get; set; }

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
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }
        }
    }
}
