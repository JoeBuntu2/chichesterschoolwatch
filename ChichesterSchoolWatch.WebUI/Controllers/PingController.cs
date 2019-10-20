using Microsoft.AspNetCore.Mvc; 

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpPost]
        public object Get(string url)
        {
            return new {Pong = "Pong"};
        }
    }
}
