using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO.Revenue;
using SchoolWatch.Business.Interface;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueCodesController : ControllerBase
    {
        private readonly IRevenueCodesService RevenueCodesService;
        private readonly ILogger<RevenueCodesController> Logger;

        public RevenueCodesController(
            IRevenueCodesService revenueCodesService,
            ILogger<RevenueCodesController> logger)
        {
            RevenueCodesService = revenueCodesService;
            Logger = logger;
        }

        // - 8 hour cache
        [HttpGet] 
        [ResponseCache(CacheProfileName = "StaticData12Hr")]
        public AllRevenueCodesDto GetCodes()
        {
            Logger.LogDebug("Made it past cache...fetching data");
            return RevenueCodesService.GetAll();
        }
    }
}