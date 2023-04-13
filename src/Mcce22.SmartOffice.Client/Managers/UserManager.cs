using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IUserManager
    {
        Task<UserModel[]> GetList();

        Task<UserModel> Save(UserModel user);

        Task Delete(int userId);
    }

    public class UserManager : ManagerBase<UserModel>, IUserManager
    {
        public UserManager(string baseUrl)
            : base($"{baseUrl}/user")
        {
        }
    }
}
