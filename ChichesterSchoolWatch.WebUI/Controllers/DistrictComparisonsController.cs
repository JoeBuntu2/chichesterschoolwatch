using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO.DistrictComparisons; 
using SchoolWatch.Business.Interface;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictComparisonsController : ControllerBase
    {
        private readonly IDistrictComparisonService DistrictComparisonService;
        private readonly ILogger<DistrictComparisonsController> Logger;

        public DistrictComparisonsController(
            IDistrictComparisonService districtComparisonService,
            ILogger<DistrictComparisonsController> logger)
        {
            DistrictComparisonService = districtComparisonService;
            Logger = logger;
        }

        // - 8 hour cache
        [HttpGet] 
        [ResponseCache(CacheProfileName = "StaticData12Hr")]
        public AllDistrictComparisonsDto Get( )
        {
            Logger.LogDebug("Made it past cache...fetching data");
            return DistrictComparisonService.GetAll();
        }
    }
}