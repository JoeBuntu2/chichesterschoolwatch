using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO.Revenue;
using SchoolWatch.Business.Interface;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenuesController: ControllerBase
    {
        private readonly IBudgetRevenuesService BudgetRevenuesService;
        private readonly ILogger<RevenuesController> Logger;

        public RevenuesController(
            IBudgetRevenuesService budgetRevenuesService,
            ILogger<RevenuesController> logger)
        {
            BudgetRevenuesService = budgetRevenuesService;
            Logger = logger;
        }

        // GET: api/Revenues - 8 hour cache
        [HttpGet]
        [ResponseCache(CacheProfileName = "StaticData12Hr")]
        public IEnumerable<DistrictRevenuesDto> Get()
        {
            Logger.LogDebug("Made it past cache...fetching data");
            return BudgetRevenuesService.GetAll();
        }  
    }
}
