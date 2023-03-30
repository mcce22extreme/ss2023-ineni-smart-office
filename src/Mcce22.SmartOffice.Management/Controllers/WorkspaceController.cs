using Mcce22.SmartOffice.Management.Managers;
using Mcce22.SmartOffice.Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceManager _workspaceManager;

        public WorkspaceController(IWorkspaceManager workspaceManager)
        {
            _workspaceManager = workspaceManager;
        }

        [HttpGet]
        public async Task<WorkspaceModel[]> GetWorkspaces()
        {
            return await _workspaceManager.GetWorkspaces();
        }

        [HttpGet("{workspaceId}")]
        public async Task<WorkspaceModel> GetWorkspace(int workspaceId)
        {
            return await _workspaceManager.GetWorkspace(workspaceId);
        }

        [HttpPost]
        public async Task<WorkspaceModel> CreateWorkspace([FromBody] SaveWorkspaceModel model)
        {
            return await _workspaceManager.CreateWorkspace(model);
        }

        [HttpPut("{workspaceId}")]
        public async Task<WorkspaceModel> UpdateWorkspace(int workspaceId, [FromBody] SaveWorkspaceModel model)
        {
            return await _workspaceManager.UpdateWorkspace(workspaceId, model);
        }

        [HttpDelete("{workspaceId}")]
        public async Task DeleteWorkspace(int workspaceId)
        {
            await _workspaceManager.DeleteWorkspace(workspaceId);
        }
    }
}
