using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.Interface;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenditureCodesController : ControllerBase
    {
        private readonly IExpenditureCodesService ExpenditureCodesService;
        private readonly ILogger<ExpenditureCodesController> Logger;

        public ExpenditureCodesController(
            IExpenditureCodesService expenditureCodesService,
            ILogger<ExpenditureCodesController> logger)
        {
            ExpenditureCodesService = expenditureCodesService;
            Logger = logger;
        }

        [HttpGet] 
        [ResponseCache(CacheProfileName = "StaticData12Hr")]
        public AllExpenditureCodesDto GetCodes()
        {
            Logger.LogDebug("Made it past cache...fetching data");
            return ExpenditureCodesService.GetAll();
        }
    }
}
