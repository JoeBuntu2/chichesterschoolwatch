using System.Collections.Generic;
using System.Linq;
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers;

namespace SchoolWatch.Business
{
    public class DistrictComparisonService : IDistrictComparisonService
    { 
        private readonly IDistrictsService DistrictsService; 
        private readonly IFiscalYearsService FiscalYearsService;
        private readonly IEnumerable<IDistrictComparer> DistrictComparers;

        public DistrictComparisonService( 
            IDistrictsService districtsService, 
            IFiscalYearsService fiscalYearsService,
            IEnumerable<IDistrictComparer> districtComparers
            )
        { 
            DistrictsService = districtsService; 
            FiscalYearsService = fiscalYearsService;
            DistrictComparers = districtComparers;
        }
 
        public AllDistrictComparisonsDto GetAll()
        {
            var response = CreateResponseObject();
            foreach (var districtComparer in DistrictComparers)
            {
                districtComparer.LoadComparisonData(response);
            }

            return response;
        }

        public AllDistrictComparisonsDto CreateResponseObject()
        {
            var response = new AllDistrictComparisonsDto
            {
                DistrictFiscalYearMetrics = new List<DistrictComparisonDto>()
            };

            //get fiscal years
            var fiscalYears = FiscalYearsService.GetSupportedYears();
            response.FiscalYears = fiscalYears.ToDictionary(x => x.FiscalYearId, x => x.Name);

            //create comparison record for every district
            var districts = DistrictsService.GetAll();
            foreach (var district in districts)
            {
                var districtData = new DistrictComparisonDto
                {
                    District = district,
                    MetricsByFiscalYear = fiscalYears.ToDictionary(x => x.FiscalYearId, x =>  new DistrictFiscalYearMetrics
                    {
                        Metrics = new Dictionary<ComparisonType, object>()
                    })
                };
                response.DistrictFiscalYearMetrics.Add(districtData);
            }

            return response;
        }
 
    }
}
