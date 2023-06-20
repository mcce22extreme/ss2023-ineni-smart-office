using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IWorkspaceConfigurationManager
    {
        Task<WorkspaceConfigurationModel[]> GetList();

        Task<WorkspaceConfigurationModel> Save(WorkspaceConfigurationModel user);

        Task Delete(string userWorkspaceId);
    }

    public class WorkspaceConfigurationManager : ManagerBase<WorkspaceConfigurationModel>, IWorkspaceConfigurationManager
    {
        public WorkspaceConfigurationManager(string baseUrl)
            : base($"{baseUrl}/workspaceconfiguration/")
        {
        }
    }
}
