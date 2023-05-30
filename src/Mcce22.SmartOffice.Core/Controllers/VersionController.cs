using Mcce22.SmartOffice.Core.Common;
using Mcce22.SmartOffice.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mcce22.SmartOffice.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        private readonly IAppInfo _appInfo;

        public VersionController(IAppInfo appInfo)
        {
            _appInfo = appInfo;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetVersionInfo()
        {
            return Ok(new VersionInfo
            {
                AppName = _appInfo.AppName,
                AppVersion = _appInfo.AppVersion,
            });
        }
    }
}
