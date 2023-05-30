using Mcce22.SmartOffice.DataIngress.Models;

namespace Mcce22.SmartOffice.DataIngress.Managers
{
    public interface IWorkspaceDataManager
    {
        Task CreateWorkspaceData(SaveWorkspaceDataModel model);
    }
}
