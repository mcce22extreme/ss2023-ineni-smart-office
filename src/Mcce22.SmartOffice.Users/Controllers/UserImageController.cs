using Mcce22.SmartOffice.Users.Managers;
using Mcce22.SmartOffice.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserImageController : ControllerBase
    {
        private readonly IUserImageManager _userImageManager;

        public UserImageController(IUserImageManager userImageManager)
        {
            _userImageManager = userImageManager;
        }

        [HttpGet]
        public Task<UserImageModel[]> GetUserImages(int userId)
        {
            return _userImageManager.GetUserImages(userId);
        }

        [HttpPost]
        public async Task<UserImageModel> CreateUserImage([FromBody] SaveUserImageModel model)
        {
            return await _userImageManager.CreateUserImage(model);
        }

        [HttpPost("{userImageId}/content")]
        public async Task<UserImageModel> StoreUserImageContent(int userImageId)
        {
            return await _userImageManager.StoreUserImageContent(userImageId, Request.Body);
        }

        [HttpDelete("{userImageId}")]
        public async Task DeleteUserImage(int userImageId)
        {
            await _userImageManager.DeleteUserImage(userImageId);
        }
    }
}
