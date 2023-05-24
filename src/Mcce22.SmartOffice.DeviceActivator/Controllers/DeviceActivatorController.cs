using Mcce22.SmartOffice.DeviceActivator.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.DeviceActivator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceActivatorController : ControllerBase
    {
        private readonly IDeviceManager _deviceManager;

        public DeviceActivatorController(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        [HttpGet("activate")]
        public async Task<IActionResult> ActivateWorkspace(string activationCode = null)
        {
            await _deviceManager.ActivateDevice(activationCode);

            return Ok("The booking has been successfully activated. You can now close the page.");
        }
    }
}
