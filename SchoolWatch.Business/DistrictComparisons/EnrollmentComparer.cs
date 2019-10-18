using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers;

namespace SchoolWatch.Business.DistrictComparisons
{
    public class EnrollmentComparer : IDistrictComparer
    {
        private readonly IBudgetsService BudgetsService;
        private readonly IEnrollmentsService EnrollmentService;
        private readonly ILogger<EnrollmentComparer> Logger;

        public EnrollmentComparer(
            IBudgetsService budgetsService, 
            IEnrollmentsService enrollmentService,
            ILogger<EnrollmentComparer> logger)
        {
            BudgetsService = budgetsService;
            EnrollmentService = enrollmentService;
            Logger = logger;
        }

        public void LoadComparisonData(AllDistrictComparisonsDto response)
        {
            BudgetsService.Prime();

            foreach (var districtContainer in response.DistrictFiscalYearMetrics)
            { 
                //for each fiscal year
                foreach (var fyMetric in districtContainer.MetricsByFiscalYear)
                {
                    var budget = BudgetsService.Find(districtContainer.District.DistrictId, fyMetric.Key);
                    var previousBudget = BudgetsService.FindPreviousYear(budget);
                   
                    //we need current and previous to calculate any increases...
                    if (budget == null || previousBudget == null)
                    {
                        Logger.LogDebug($"Skipping for district {districtContainer.District.Name}. Budget Null?:{budget == null}. Previous Budget Null?:({previousBudget == null}");
                        continue;
                    }

                    var fyEnrollment = EnrollmentService.Find(districtContainer.District.DistrictId, budget.FiscalYearId);
 

                    //add the metric
                    fyMetric.Value.Metrics.Add(ComparisonType.Enrollment, fyEnrollment.Enrollment); 
                }
            }
        }
    }
}
