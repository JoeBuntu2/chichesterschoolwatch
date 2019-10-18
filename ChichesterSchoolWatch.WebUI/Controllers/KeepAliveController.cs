using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWatch.Data;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeepAliveController : ControllerBase
    {
        private readonly IDataEngine DataEngine;
        private readonly ILogger<KeepAliveController> Logger;

        public KeepAliveController(IDataEngine dataEngine, ILogger<KeepAliveController> logger)
        {
            DataEngine = dataEngine;
            Logger = logger;
        }

        public void Get()
        {
            //my only purpose is to keep the connection pool loaded...
            Logger.LogDebug("Keep Alive Called");
            DataEngine.KeepAlive();
        }
    }
}