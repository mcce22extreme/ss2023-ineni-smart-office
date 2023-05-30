using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Workspaces.Managers
{
    public interface IWorkspaceManager
    {
        Task<WorkspaceModel[]> GetWorkspaces();

        Task<WorkspaceModel> GetWorkspace(string workspaceId);

        Task<WorkspaceModel> CreateWorkspace(SaveWorkspaceModel model);

        Task<WorkspaceModel> UpdateWorkspace(string workspaceId, SaveWorkspaceModel model);

        Task DeleteWorkspace(string workspaceId);
    }
}
