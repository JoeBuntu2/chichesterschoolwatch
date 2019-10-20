using System.Linq;
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers;

namespace SchoolWatch.Business.DistrictComparisons
{
    /// <summary>
    /// Revenue Per Student 
    /// </summary>
    public class TotalRevenueComparer : IDistrictComparer
    {
        private readonly IBudgetsService BudgetsService;
        private readonly IBudgetRevenuesService BudgetRevenuesService;
        private readonly IEnrollmentsService EnrollmentsService;

        public TotalRevenueComparer(
            IBudgetsService budgetsService,
            IBudgetRevenuesService budgetRevenuesService, 
            IEnrollmentsService enrollmentsService)
        {
            BudgetsService = budgetsService;
            BudgetRevenuesService = budgetRevenuesService;
            EnrollmentsService = enrollmentsService;
        }

        public void LoadComparisonData(AllDistrictComparisonsDto response)
        {
            var revenueByDistrict = BudgetRevenuesService.GetAll().ToDictionary(x => x.DistrictId);
            var enrollmentsByDistrict = EnrollmentsService.GetAll().ToDictionary(x => x.DistrictId);
            BudgetsService.Prime();

            foreach (var districtContainer in response.DistrictFiscalYearMetrics)
            {
                //if we don't have revenues for this district... pass
                if(!revenueByDistrict.ContainsKey(districtContainer.District.DistrictId))
                    continue;

                //if we don't have enrollment data for this district... pass
                if(!enrollmentsByDistrict.ContainsKey(districtContainer.District.DistrictId))
                    continue;
 
                //get revenues for this district
                var districtRevenues = revenueByDistrict[districtContainer.District.DistrictId];
                var fiscalYearRevenues = districtRevenues.FiscalYearAmounts.ToDictionary(x => x.FiscalYearId);
                var levelAmounts = districtRevenues.Sources.ToDictionary(x => x.LevelId);

                //for each fiscal year
                foreach (var fyMetric in districtContainer.MetricsByFiscalYear)
                {


                    //if we don't have revenue for this year... pass
                    if(!fiscalYearRevenues.ContainsKey(fyMetric.Key))
                        continue;
 
                    //calculate the revenue per student based on average enrollment...
                    var fyRevenue = (decimal) fiscalYearRevenues[fyMetric.Key].Total;
                    var enrollment = enrollmentsByDistrict[districtContainer.District.DistrictId];
                    var revenuePerStudent = fyRevenue / enrollment.Enrollment;

                    //add the metric
                    fyMetric.Value.Metrics.Add(ComparisonType.TotalRevenue, fyRevenue);
                    fyMetric.Value.Metrics.Add(ComparisonType.TotalRevenuePerStudent, revenuePerStudent);

                    //try to get state revenues
                    if (levelAmounts.TryGetValue("S", out var stateRevenues))
                    {
                        var currentYearStateRevenue = stateRevenues.FiscalYearAmounts.FirstOrDefault(x => x.FiscalYearId == fyMetric.Key);
                        if (currentYearStateRevenue != null)
                        {
                            var fyStateRevenue = currentYearStateRevenue.Total;
                            fyMetric.Value.Metrics.Add(ComparisonType.StateRevenue, fyStateRevenue);

                            var stateRevenuePerStudent = currentYearStateRevenue.Total / enrollment.Enrollment;
                            fyMetric.Value.Metrics.Add(ComparisonType.StateRevenuePerStudent, stateRevenuePerStudent);

                            fyMetric.Value.Metrics.Add(ComparisonType.StateRevenuePercent, fyStateRevenue / fyRevenue);
                        }
                    }
 

 
                    //now make comparisons to previous year
                    var budget = BudgetsService.Find(districtContainer.District.DistrictId, fyMetric.Key);
                    var previousBudget = BudgetsService.FindPreviousYear(budget);

                    //only make comparison if we have data
                    if(previousBudget == null)
                        continue;

                    if(!fiscalYearRevenues.ContainsKey(previousBudget.FiscalYearId))
                        continue;

                    var previousYearsRevenue = (decimal) fiscalYearRevenues[previousBudget.FiscalYearId].Total;

                    var revenueIncrease = fyRevenue - previousYearsRevenue;
                    var revenuePerStudentIncrease = revenueIncrease / enrollment.Enrollment;
                    

                    fyMetric.Value.Metrics.Add(ComparisonType.RevenueIncrease, revenueIncrease);
                    fyMetric.Value.Metrics.Add(ComparisonType.RevenueIncreasePerStudent, revenuePerStudentIncrease);
                }
            }
        }
    }
}