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

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<UserModel[]> GetUsers()
        {
            return await _userManager.GetUsers();
        }

        [HttpGet("{userId}")]
        public async Task<UserModel> GetUser(int userId)
        {
            return await _userManager.GetUser(userId);
        }

        [HttpPost]
        public async Task<UserModel> CreateUser([FromBody] SaveUserModel model)
        {
            return await _userManager.CreateUser(model);
        }

        [HttpPut("{userId}")]
        public async Task<UserModel> UpdateUser(int userId, [FromBody] SaveUserModel model)
        {
            return await _userManager.UpdateUser(userId, model);
        }

        [HttpDelete("{userId}")]
        public async Task DeleteUser(int userId)
        {
            await _userManager.DeleteUser(userId);
        }
    }
}
