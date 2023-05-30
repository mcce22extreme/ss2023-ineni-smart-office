using Mcce22.SmartOffice.Core.Common;

namespace Mcce22.SmartOffice.Core.Providers
{
    public interface IAppConfigProvider
    {
        Task<AppConfig> GetAppConfig();
    }
}
