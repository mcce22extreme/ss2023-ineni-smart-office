using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using Serilog;

namespace Mcce22.SmartOffice.Users
{
    public interface IAppSettings
    {
        string BucketName { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public string BaseAddress { get; set; }

        public string SecretName { get; set; }

        public string BucketName { get; set; }

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

                    BucketName = settings.BucketName;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }
        }
    }
}
