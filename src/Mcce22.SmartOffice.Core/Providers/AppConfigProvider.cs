using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace Mcce22.SmartOffice.Core.Providers
{
    public interface IAppConfigProvider
    {
        Task<AppConfig> GetAppConfig();
    }

    public class AppConfigProvider : IAppConfigProvider
    {
        private const string IMAGE_BUCKETNAME_PARAMETER = "ImageBucketName";
        private const string SMTP_HOST_PARAMETER = "SmtpHost";
        private const string SMTP_PORT_PARAMETER = "SmtpPort";
        private const string SMTP_USERNAME_PARAMETER = "SmtpUsername";
        private const string SMTP_PASSWORD_PARAMETER = "SmtpPassword";
        private const string SMTP_SENDERNAME_PARAMETER = "SmtpSendername";
        private const string ACTIVATOR_ENDPOINTADDRESS_PARAMETER = "ActivatorEndpointAddress";
        private const string IOTDATA_ENDPOINTADDRESS_PARAMETER = "IoTDataEndpointAddress";

        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

        private readonly IAmazonSimpleSystemsManagement _systemsManager;

        private AppConfig _appConfig;

        public AppConfigProvider(IAmazonSimpleSystemsManagement systemsManager)
        {
            _systemsManager = systemsManager;
        }


        public async Task<AppConfig> GetAppConfig()
        {
            await Semaphore.WaitAsync();

            try
            {
                if (_appConfig == null)
                {
                    _appConfig = new AppConfig();

                    var result = await _systemsManager.GetParametersAsync(new GetParametersRequest
                    {
                        Names = new List<string>
                        {
                            IMAGE_BUCKETNAME_PARAMETER,
                            SMTP_HOST_PARAMETER,
                            SMTP_PORT_PARAMETER,
                            SMTP_USERNAME_PARAMETER,
                            SMTP_PASSWORD_PARAMETER,
                            SMTP_SENDERNAME_PARAMETER,
                            ACTIVATOR_ENDPOINTADDRESS_PARAMETER,
                            IOTDATA_ENDPOINTADDRESS_PARAMETER
                        }
                    });

                    foreach (var parameter in result.Parameters)
                    {
                        switch (parameter.Name)
                        {
                            case IMAGE_BUCKETNAME_PARAMETER:
                                _appConfig.ImageBucketName = parameter.Value;
                                break;
                            case SMTP_HOST_PARAMETER:
                                _appConfig.SmtpHost = parameter.Value;
                                break;
                            case SMTP_PORT_PARAMETER:
                                _appConfig.SmtpPort = int.Parse(parameter.Value);
                                break;
                            case SMTP_USERNAME_PARAMETER:
                                _appConfig.SmtpUserName = parameter.Value;
                                break;
                            case SMTP_PASSWORD_PARAMETER:
                                _appConfig.SmtpPassword = parameter.Value;
                                break;
                            case SMTP_SENDERNAME_PARAMETER:
                                _appConfig.SmtpSenderName = parameter.Value;
                                break;
                            case ACTIVATOR_ENDPOINTADDRESS_PARAMETER:
                                _appConfig.ActivatorEndpointAddress = parameter.Value;
                                break;
                            case IOTDATA_ENDPOINTADDRESS_PARAMETER:
                                _appConfig.IoTDataEndpointAddress = parameter.Value;
                                break;
                        }
                    }
                }

                return _appConfig;
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }

    public class AppConfig
    {
        public string ImageBucketName { get; set; }

        public string IoTDataEndpointAddress { get; set; }

        public string ActivatorEndpointAddress { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUserName { get; set; }

        public string SmtpPassword { get; set; }

        public string SmtpSenderName { get; set; }
    }
}
