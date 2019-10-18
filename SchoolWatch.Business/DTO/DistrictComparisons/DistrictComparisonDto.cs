using System.Collections.Generic;

namespace SchoolWatch.Business.DTO.DistrictComparisons
{
    /// <summary>
    /// Container for district specific metrics
    /// </summary>
    public class DistrictComparisonDto
    {
        public DistrictsDto District { get; set; }

        //Metrics for each fiscal year (key=fiscal year id)
        public Dictionary<int, DistrictFiscalYearMetrics> MetricsByFiscalYear { get; set; }
    }
}
