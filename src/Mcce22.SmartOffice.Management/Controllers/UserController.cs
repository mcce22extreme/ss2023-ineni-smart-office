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

        /// <summary>
        /// Retrieve a list of available users.
        /// </summary>
        /// <response code="200">Users retrieved successfully.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to retrieve users.</response>
        [HttpGet]
        public async Task<UserModel[]> GetUsers()
        {
            return await _userManager.GetUsers();
        }

        /// <summary>
        /// Retrieve a specific user.
        /// </summary>
        /// <param name="userId" example="1">The user id.</param>
        /// <returns>The user with the given id.</returns>
        /// <response code="200">User retrieved successfully.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to retrieve users.</response>
        /// <response code="404">User with the given id was not found.</response>
        [HttpGet("{userId}")]
        public async Task<UserModel> GetUser(int userId)
        {
            return await _userManager.GetUser(userId);
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="model">Information of the new user.</param>
        /// <returns>The new created user.</returns>
        /// <response code="200">User created successfully.</response>
        /// <response code="400">Validation error occured.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to create users.</response>
        [HttpPost]
        public async Task<UserModel> CreateUser([FromBody] SaveUserModel model)
        {
            return await _userManager.CreateUser(model);
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="userId" example="1">The user id.</param>
        /// <param name="model">Information of the user.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">User updated successfully.</response>
        /// <response code="400">Validation error occured.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to update user.</response>
        /// <response code="404">User with the given id was not found.</response>
        [HttpPut("{userId}")]
        public async Task<UserModel> UpdateUser(int userId, [FromBody] SaveUserModel model)
        {
            return await _userManager.UpdateUser(userId, model);
        }

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <param name="userId" example="1">The user id.</param>
        /// <response code="200">User deleted successfully.</response>        
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to delete users.</response>
        /// <response code="404">User with the given id was not found.</response>
        [HttpDelete("{userId}")]
        public async Task DeleteUser(int userId)
        {
            await _userManager.DeleteUser(userId);
        }
    }
}
