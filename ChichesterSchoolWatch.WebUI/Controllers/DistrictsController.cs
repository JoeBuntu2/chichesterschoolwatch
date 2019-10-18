using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business;
using SchoolWatch.Business.Interface;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictsService DistrictsService;
        private readonly ILogger<DistrictsController> Logger;

        public DistrictsController(
            IDistrictsService districtsService,
            ILogger<DistrictsController> logger)
        {
            DistrictsService = districtsService;
            Logger = logger;
        }

        // GET: api/Revenues - 5 hour cache
        [HttpGet]
        [ResponseCache(CacheProfileName = "StaticData12Hr")]
        public IEnumerable<DistrictsDto> Get()
        {
            Logger.LogDebug("Made it past cache...fetching data");
            return DistrictsService.GetAll();
        } 
    }
}