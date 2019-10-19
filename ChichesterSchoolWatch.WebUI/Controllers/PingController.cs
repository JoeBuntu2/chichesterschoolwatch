using Microsoft.AspNetCore.Mvc; 

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    { 
        [HttpPost] 
        public string Get(string url)
        {
            return "Pong";
        } 
    }
}
