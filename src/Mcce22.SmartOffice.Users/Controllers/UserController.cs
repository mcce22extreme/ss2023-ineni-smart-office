using Mcce22.SmartOffice.Users.Managers;
using Mcce22.SmartOffice.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Users.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IUserImageManager _userImageManager;

        public UserController(IUserManager userManager, IUserImageManager userImageManager)
        {
            _userManager = userManager;
            _userImageManager = userImageManager;
        }

        [HttpGet]
        public async Task<UserModel[]> GetUsers()
        {
            return await _userManager.GetUsers();
        }

        [HttpGet("{userId}")]
        public async Task<UserModel> GetUser(string userId)
        {
            return await _userManager.GetUser(userId);
        }

        [HttpPost]
        public async Task<UserModel> CreateUser([FromBody] SaveUserModel model)
        {
            return await _userManager.CreateUser(model);
        }

        [HttpPut("{userId}")]
        public async Task<UserModel> UpdateUser(string userId, [FromBody] SaveUserModel model)
        {
            return await _userManager.UpdateUser(userId, model);
        }

        [HttpDelete("{userId}")]
        public async Task DeleteUser(string userId)
        {
            await _userManager.DeleteUser(userId);
        }
    }
}
