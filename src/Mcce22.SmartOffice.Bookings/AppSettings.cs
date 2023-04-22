namespace Mcce22.SmartOffice.Bookings
{
    public class AppSettings
    {
        public string BaseAddress { get; set; }

        //public SmptConfiguration SmptConfiguration { get; set; } = new SmptConfiguration();

        //public async Task LoadConfigFromAWSSecretsManager()
        //{
        //    if (!string.IsNullOrEmpty(SecretId))
        //    {
        //        using var client = new AmazonSecretsManagerClient();

        //        var request = new GetSecretValueRequest
        //        {
        //            SecretId = SecretId
        //        };

        //        try
        //        {
        //            var response = await client.GetSecretValueAsync(request);

        //            var settings = JsonConvert.DeserializeObject<AppSettings>(response.SecretString);

        //            ConnectionString = settings.ConnectionString;
        //            SmptConfiguration = settings.SmptConfiguration;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex, ex.Message);
        //        }
        //    }
        //}
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
