using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Managers
{
    public interface IUserImageManager
    {
        Task<UserImageModel[]> GetUserImages(string userId);

        Task<UserImageModel> StoreUserImage(string userId, IFormFile file);

        Task DeleteUserImage(string userImageId);
    }
}
