using System.Linq;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers; 

namespace SchoolWatch.Business.DistrictComparisons
{
    /// <summary>
    /// The shit the board members know and love...
    /// </summary>
    public class PropertyAssessmentComparer : IDistrictComparer
    {
        private readonly IBudgetsService BudgetsService;
        private readonly IEnrollmentsService EnrollmentsService;

        public PropertyAssessmentComparer(IBudgetsService budgetsService, IEnrollmentsService enrollmentsService)
        {
            BudgetsService = budgetsService;
            EnrollmentsService = enrollmentsService;
        }

        public void LoadComparisonData(AllDistrictComparisonsDto response)
        { 
            var enrollments = EnrollmentsService.GetAll().ToDictionary(x => x.DistrictId);

            foreach (var districtContainer in response.DistrictFiscalYearMetrics)
            { 
                //for each fiscal year
                foreach (var fyMetric in districtContainer.MetricsByFiscalYear)
                {
                    //only proceed if we have a budget
                    var budget = BudgetsService.Find(districtContainer.District.DistrictId, fyMetric.Key);
                    if (budget == null || budget.Assessed.HasValue == false)
                        continue;

                    //add assessment
                    fyMetric.Value.Metrics.Add(ComparisonType.Assessed, budget.Assessed);

                    //if we have enrollment, calculate assessment per student
                    if (enrollments.TryGetValue(districtContainer.District.DistrictId, out var enrollment))
                    {
                        var assessedPerStudent = budget.Assessed.Value / enrollment.Enrollment;
                        fyMetric.Value.Metrics.Add(ComparisonType.AssessedPerStudent, assessedPerStudent);
                    }
 
                    //we can't do comparisons to previous w/out a previous budget
                    var previousBudget = BudgetsService.FindPreviousYear(budget);
                    if( previousBudget == null || previousBudget.Assessed.HasValue == false)
                        continue;
 
                    var assessedChanged = budget.Assessed - previousBudget.Assessed;
                    var assessedNewRevenue = (assessedChanged * budget.Millage) / 1000;

                    //add the metric
                    fyMetric.Value.Metrics.Add(ComparisonType.AssessedIncrease, assessedChanged);
                    fyMetric.Value.Metrics.Add(ComparisonType.AssessedNewRevenue, assessedNewRevenue);
 
                    if(enrollment == null)
                        continue;

                    var assessedNewRevenuePerStudent = assessedNewRevenue / enrollment.Enrollment;
                    fyMetric.Value.Metrics.Add(ComparisonType.AssessedNewRevenuePerStudent, assessedNewRevenuePerStudent);
                    
                }
            }
        }
    }
}
