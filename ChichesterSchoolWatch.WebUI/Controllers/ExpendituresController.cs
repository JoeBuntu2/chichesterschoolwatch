using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.Interface;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpendituresController : ControllerBase
    {
        private readonly IBudgetExpendituresService BudgetExpendituresService;
        private readonly ILogger<ExpendituresController> Logger;

        public ExpendituresController(
            IBudgetExpendituresService budgetExpendituresService,
            ILogger<ExpendituresController> logger)
        {
            BudgetExpendituresService = budgetExpendituresService;
            Logger = logger;
        }

        // GET: api/Revenues - 8 hour cache
        [HttpGet]
        [ResponseCache(CacheProfileName = "StaticData12Hr")]
        public IEnumerable<DistrictExpendituresDto> Get()
        {
            Logger.LogDebug("Made it past cache...fetching data");
            return BudgetExpendituresService.GetAll();
        } 


 
    }
}
