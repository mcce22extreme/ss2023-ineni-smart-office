using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Workspaces.Managers;
using Mcce22.SmartOffice.Workspaces.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Workspaces.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IWorkspaceConfigurationManager _configurationManager;

        public WorkspaceController(IWorkspaceManager workspaceManager, IWorkspaceConfigurationManager configurationManager)
        {
            _workspaceManager = workspaceManager;
            _configurationManager = configurationManager;
        }

        [HttpGet]
        public async Task<WorkspaceModel[]> GetWorkspaces()
        {
            return await _workspaceManager.GetWorkspaces();
        }

        [HttpGet("{workspaceId}")]
        public async Task<WorkspaceModel> GetWorkspace(string workspaceId)
        {
            return await _workspaceManager.GetWorkspace(workspaceId);
        }

        [HttpPost]
        public async Task<WorkspaceModel> CreateWorkspace([FromBody] SaveWorkspaceModel model)
        {
            return await _workspaceManager.CreateWorkspace(model);
        }

        [HttpPut("{workspaceId}")]
        public async Task<WorkspaceModel> UpdateWorkspace(string workspaceId, [FromBody] SaveWorkspaceModel model)
        {
            return await _workspaceManager.UpdateWorkspace(workspaceId, model);
        }

        [HttpDelete("{workspaceId}")]
        public async Task DeleteWorkspace(string workspaceId)
        {
            await _workspaceManager.DeleteWorkspace(workspaceId);
            await _configurationManager.DeleteWorkspaceConfigurationsForWorkspace(workspaceId);
        }
    }
}
