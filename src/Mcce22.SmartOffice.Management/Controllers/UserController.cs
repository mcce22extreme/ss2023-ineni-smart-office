using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController
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
