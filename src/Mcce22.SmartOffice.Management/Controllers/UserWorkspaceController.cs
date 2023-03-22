using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Models;
using Mcce22.SmartOffice.Management.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Management.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserWorkspaceController : ControllerBase
    {
        private readonly IUserWorkspaceManager _userWorkspaceManager;

        public UserWorkspaceController(IUserWorkspaceManager userWorkspaceManager)
        {
            _userWorkspaceManager = userWorkspaceManager;
        }

        [HttpGet]
        public async Task<UserWorkspaceModel> GetUserWorkspace([FromQuery] UserWorkspaceQuery query)
        {
            return await _userWorkspaceManager.GetUserWorkspace(query.UserId, query.WorkspaceId);
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
