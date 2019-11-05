using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO.DistrictComparisons; 
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Something;

namespace ChichesterSchoolWatch.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictComparisonsController : ControllerBase
    {
        private readonly IDistrictComparisonService DistrictComparisonService;
        private readonly ISomeNewService SomeNewService;
        private readonly ILogger<DistrictComparisonsController> Logger;

        public DistrictComparisonsController(
            IDistrictComparisonService districtComparisonService,
            ISomeNewService someNewService,
            ILogger<DistrictComparisonsController> logger)
        {
            DistrictComparisonService = districtComparisonService;
            SomeNewService = someNewService;
            Logger = logger;
        }
 
        [HttpGet] 
        [ResponseCache(CacheProfileName = "StaticData12Hr")]
        public AllDistrictComparisonsDto Get( )
        {
            Logger.LogDebug("Made it past cache...fetching data");
            return DistrictComparisonService.GetAll();
        }
 
        [HttpGet("download")]
        public List<DownloadRecord> Download( ComparisonType comparisonType)
        { 
            
            var all = DistrictComparisonService.GetAll();

            var results = new List<DownloadRecord>();
            foreach (var allDistrictFiscalYearMetric in all.DistrictFiscalYearMetrics)
            {
                foreach (var fiscalYearMetric in allDistrictFiscalYearMetric.MetricsByFiscalYear)
                {
                    var value = fiscalYearMetric.Value.Metrics[comparisonType];

                    results.Add(new DownloadRecord
                    {
                        Disrict = allDistrictFiscalYearMetric.District.Name,
                        FiscalYear = all.FiscalYears[fiscalYearMetric.Key],
                        Value = value
                    });
                }
            }

            return results;
        }

        [HttpGet("somethingNew")]
        public List<DistrictAmounts> SomethingNew(string district1, string district2)
        {
            return SomeNewService.CompareTwoBudgets(district1, district2);
        }

        public class DownloadRecord
        {
            public string Disrict { get; set; }
            public string FiscalYear { get; set; }
            public object Value { get; set; }
        }


    }
}