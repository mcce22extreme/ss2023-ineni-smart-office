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

        /// <summary>
        /// Retrieve a list of available workspaces.
        /// </summary>
        /// <response code="200">Workspaces retrieved successfully.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to retrieve workspaces.</response>
        [HttpGet]
        public async Task<WorkspaceModel[]> GetWorkspaces()
        {
            return await _workspaceManager.GetWorkspaces();
        }

        /// <summary>
        /// Retrieve a specific workspace.
        /// </summary>
        /// <param name="workspaceId" example="1">The workspace id.</param>
        /// <returns>The workspace with the given id.</returns>
        /// <response code="200">Workspace retrieved successfully.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to retrieve workspaces.</response>
        /// <response code="404">Workspace with the given id was not found.</response>
        [HttpGet("{workspaceId}")]
        public async Task<WorkspaceModel> GetWorkspace(int workspaceId)
        {
            return await _workspaceManager.GetWorkspace(workspaceId);
        }

        /// <summary>
        /// Create a new workspace.
        /// </summary>
        /// <param name="model">Information of the new workspace.</param>
        /// <returns>The new created workspace.</returns>
        /// <response code="200">Workspace created successfully.</response>
        /// <response code="400">Validation error occured.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to create workspaces.</response>
        [HttpPost]
        public async Task<WorkspaceModel> CreateWorkspace([FromBody] SaveWorkspaceModel model)
        {
            return await _workspaceManager.CreateWorkspace(model);
        }

        /// <summary>
        /// Update an existing workspace.
        /// </summary>
        /// <param name="workspaceId" example="1">The workspace id.</param>
        /// <param name="model">Information of the workspace.</param>
        /// <returns>The updated workspace.</returns>
        /// <response code="200">Workspace updated successfully.</response>
        /// <response code="400">Validation error occured.</response>
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to update workspace.</response>
        /// <response code="404">Workspace with the given id was not found.</response>
        [HttpPut("{workspaceId}")]
        public async Task<WorkspaceModel> UpdateWorkspace(int workspaceId, [FromBody] SaveWorkspaceModel model)
        {
            return await _workspaceManager.UpdateWorkspace(workspaceId, model);
        }

        /// <summary>
        /// Delete an existing workspace.
        /// </summary>
        /// <param name="workspaceId" example="1">The workspace id.</param>
        /// <response code="200">Workspace deleted successfully.</response>        
        /// <response code="401">No authentication information provided.</response>
        /// <response code="403">Not authorized to delete workspace.</response>
        /// <response code="404">Workspace with the given id was not found.</response>
        [HttpDelete("{workspaceId}")]
        public async Task DeleteWorkspace(int workspaceId)
        {
            await _workspaceManager.DeleteWorkspace(workspaceId);
        }
    }
}
