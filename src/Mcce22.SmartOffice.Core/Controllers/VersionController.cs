using Mcce22.SmartOffice.Core.Common;
using Mcce22.SmartOffice.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Core.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionController : ControllerBase
    {
        /// <summary>
        /// Returns version information of the application.
        /// </summary>
        /// <response code="200">The version information of the application.</response>        
        [HttpGet]
        [AllowAnonymous]        
        public IActionResult GetVersionInfo()
        {
            return Ok(new VersionInfo
            {
                AppName = AppInfo.Current.AppName,
                AppVersion = AppInfo.Current.AppVersion
            });
        }
    }
}
