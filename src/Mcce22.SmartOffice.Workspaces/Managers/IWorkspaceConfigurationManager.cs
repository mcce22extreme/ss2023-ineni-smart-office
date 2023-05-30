using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Bookings.Managers
{
    public interface IWorkspaceConfigurationManager
    {
        Task<WorkspaceConfigurationModel[]> GetWorkspaceConfigurations(string userId, string workspaceId);

        Task<WorkspaceConfigurationModel> GetWorkspaceConfiguration(string configurationId);

        Task<WorkspaceConfigurationModel> CreateWorkspaceConfiguration(SaveWorkspaceConfigurationModel model);

        Task<WorkspaceConfigurationModel> UpdateWorkspaceConfiguration(string configurationId, SaveWorkspaceConfigurationModel model);

        Task DeleteWorkspaceConfiguration(string configurationId);

        Task DeleteWorkspaceConfigurationsForUser(string userId);

        Task DeleteWorkspaceConfigurationsForWorkspace(string workspaceId);
    }
}
