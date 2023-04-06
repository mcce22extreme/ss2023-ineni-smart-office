using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Models;
using Mcce22.SmartOffice.Management.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserWorkspaceController : ControllerBase
    {
        private readonly IUserWorkspaceManager _userWorkspaceManager;

        public UserWorkspaceController(IUserWorkspaceManager userWorkspaceManager)
        {
            _userWorkspaceManager = userWorkspaceManager;
        }

        [HttpGet]
        public async Task<UserWorkspaceModel[]> GetUserWorkspaces([FromQuery] UserWorkspaceQuery query)
        {
            return await _userWorkspaceManager.GetUserWorkspaces(query.UserId, query.WorkspaceId);
        }

        [HttpGet("{userWorkspaceId}")]
        public async Task<UserWorkspaceModel> GetUserWorkspaces(int userWorkspaceId)
        {
            return await _userWorkspaceManager.GetUserWorkspace(userWorkspaceId);
        }

        [HttpPost]
        public async Task<UserWorkspaceModel> CreateUserWorkspace([FromBody] SaveUserWorkspaceModel model)
        {
            return await _userWorkspaceManager.CreateUserWorkspace(model);
        }

        [HttpPut]
        public async Task<UserWorkspaceModel> UpdateUserWorkspace([FromBody] SaveUserWorkspaceModel model)
        {
            return await _userWorkspaceManager.UpdateUserWorkspace(model);
        }

        [HttpDelete]
        public async Task DeleteUserWorkspace([FromQuery] UserWorkspaceQuery query)
        {
            await _userWorkspaceManager.DeleteUserWorkspace(query.UserId, query.WorkspaceId);
        }
    }
}
