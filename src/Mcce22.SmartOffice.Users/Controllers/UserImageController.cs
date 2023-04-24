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

        [HttpGet("{userid}")]
        public async Task<UserImageModel[]> GetUserImages(string userId)
        {
            return await _userImageManager.GetUserImages(userId);
        }

        [HttpPost("{userid}")]
        public async Task<UserImageModel> StoreUserImage(string userId, IFormFile file)
        {
            return await _userImageManager.StoreUserImage(userId, file);
        }

        [HttpDelete("{*userImageId}")]
        public async Task DeleteUserImage(string userImageId)
        {
            await _userImageManager.DeleteUserImage(userImageId);
        }
    }
}
