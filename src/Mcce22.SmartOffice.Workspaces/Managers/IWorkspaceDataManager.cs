using Mcce22.SmartOffice.Workspaces.Models;
using Mcce22.SmartOffice.Workspaces.Queries;

namespace Mcce22.SmartOffice.Workspaces.Managers
{
    public interface IWorkspaceDataManager
    {
        Task<WorkspaceDataModel[]> GetWorkspaceData(WorkspaceDataQuery query);

        Task<WorkspaceDataModel> CreateWorkspaceData(SaveWorkspaceDataModel model);

        Task DeleteWorkspaceData(string workspaceDataId);

        Task DeleteAll();
    }
}
