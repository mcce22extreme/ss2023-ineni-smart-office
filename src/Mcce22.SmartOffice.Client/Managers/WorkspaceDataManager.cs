using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IWorkspaceDataManager
    {
        Task<WorkspaceDataModel[]> GetList();

        Task<WorkspaceDataModel> Save(WorkspaceDataModel model);

        Task DeleteAll();
    }

    public class WorkspaceDataManager : ManagerBase<WorkspaceDataModel>, IWorkspaceDataManager
    {
        public WorkspaceDataManager(string baseUrl)
            : base($"{baseUrl}/workspacedata/")
        {
        }

        public async Task DeleteAll()
        {
            var url = $"{BaseUrl}/deleteall";

            await HttpClient.DeleteAsync(url);
        }
    }
}
