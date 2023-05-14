using Mcce22.SmartOffice.Bookings.Managers;
using Mcce22.SmartOffice.Workspaces.Models;
using Mcce22.SmartOffice.Workspaces.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Workspaces.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkspaceConfigurationController : ControllerBase
    {
        private readonly IWorkspaceConfigurationManager _configurationManager;

        public WorkspaceConfigurationController(IWorkspaceConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        [HttpGet]
        public async Task<WorkspaceConfigurationModel[]> GetWorkspaceConfigurations([FromQuery] WorkspaceConfigurationQuery query)
        {
            return await _configurationManager.GetWorkspaceConfigurations(query.UserId, query.WorkspaceId);
        }

        [HttpGet("{configurationId}")]
        public async Task<WorkspaceConfigurationModel> GetWorkspaceConfiguration(string configurationId)
        {
            return await _configurationManager.GetWorkspaceConfiguration(configurationId);
        }

        [HttpPost]
        public async Task<WorkspaceConfigurationModel> CreateWorkspaceConfiguration([FromBody] SaveWorkspaceConfigurationModel model)
        {
            return await _configurationManager.CreateWorkspaceConfiguration(model);
        }

        [HttpPut("{configurationId}")]
        public async Task<WorkspaceConfigurationModel> UpdateWorkspaceConfiguration(string configurationId, [FromBody] SaveWorkspaceConfigurationModel model)
        {
            return await _configurationManager.UpdateWorkspaceConfiguration(configurationId, model);
        }

        [HttpDelete("{configurationId}")]
        public async Task DeleteWorkspaceConfiguration(string configurationId)
        {
            await _configurationManager.DeleteWorkspaceConfiguration(configurationId);
        }
    }
}
