using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IUserWorkspaceManager
    {
        Task<UserWorkspaceModel[]> GetList();

        Task<UserWorkspaceModel> Save(UserWorkspaceModel user);

        Task Delete(string userWorkspaceId);
    }

    public class UserWorkspaceManager : ManagerBase<UserWorkspaceModel>, IUserWorkspaceManager
    {
        public UserWorkspaceManager(string baseUrl)
            : base($"{baseUrl}/workspaceconfiguration")
        {
        }
    }
}
