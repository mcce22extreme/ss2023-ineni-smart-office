using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Managers
{
    public interface IUserManager
    {
        Task<UserModel[]> GetUsers();

        Task<UserModel> GetUser(string userId);

        Task<UserModel> CreateUser(SaveUserModel model);

        Task<UserModel> UpdateUser(string userId, SaveUserModel model);

        Task DeleteUser(string userId);
    }
}
