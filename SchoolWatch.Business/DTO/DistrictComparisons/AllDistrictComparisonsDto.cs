using System.Collections.Generic; 

namespace SchoolWatch.Business.DTO.DistrictComparisons
{
    
    public class AllDistrictComparisonsDto
    {
        //List of all fiscal years
        public Dictionary<int, string> FiscalYears { get; set; }

        //Container for all districts
        public List<DistrictComparisonDto> DistrictFiscalYearMetrics { get; set; }
    }
}
