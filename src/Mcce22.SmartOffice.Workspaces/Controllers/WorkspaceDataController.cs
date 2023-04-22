using Mcce22.SmartOffice.Workspaces.Managers;
using Mcce22.SmartOffice.Workspaces.Models;
using Mcce22.SmartOffice.Workspaces.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Workspaces.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkspaceDataController : ControllerBase
    {
        private readonly IWorkspaceDataManager _workspaceDataManager;

        public WorkspaceDataController(IWorkspaceDataManager workspaceDataManager)
        {
            _workspaceDataManager = workspaceDataManager;
        }

        [HttpGet]
        public async Task<WorkspaceDataModel[]> GetWorkspaceData([FromQuery] WorkspaceDataQuery query)
        {
            return await _workspaceDataManager.GetWorkspaceData(query);
        }

        [HttpPost]
        public async Task<WorkspaceDataModel> CreateWorkspaceData([FromBody] SaveWorkspaceDataModel model)
        {
            return await _workspaceDataManager.CreateWorkspaceData(model);
        }

        [HttpDelete("{workspaceDataId}")]
        public async Task DeleteWorkspaceData(string workspaceDataId)
        {
            await _workspaceDataManager.DeleteWorkspaceData(workspaceDataId);
        }
    }
}
