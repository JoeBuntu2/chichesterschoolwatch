using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Entities;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController : ControllerBase
    {
        private readonly IEmployeesService EmployeesService;
        private readonly ILogger<SalariesController> Logger;

        public SalariesController(
            IEmployeesService employeesService, 
            ILogger<SalariesController> logger)
        {
            EmployeesService = employeesService;
            Logger = logger;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "StaticData12Hr")]
        public IEnumerable<EmployeeEntity> Get()
        {
            Logger.LogDebug("Made it past cache...fetching data");
            return EmployeesService.GetAll();
        }
    }
}