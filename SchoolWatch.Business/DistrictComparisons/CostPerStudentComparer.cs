using System.Linq;
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers; 

namespace SchoolWatch.Business.DistrictComparisons
{
    /// <summary>
    /// Cost Per Student Comparison
    /// </summary>
    public class CostPerStudentComparer : IDistrictComparer
    {
        private readonly IBudgetsService BudgetsService;
        private readonly IBudgetExpendituresService BudgetExpendituresService;
        private readonly IEnrollmentsService EnrollmentsService;

        public CostPerStudentComparer(
            IBudgetsService budgetsService,
            IBudgetExpendituresService budgetExpendituresService,
            IEnrollmentsService enrollmentsService
            )
        {
            BudgetsService = budgetsService;
            BudgetExpendituresService = budgetExpendituresService;
            EnrollmentsService = enrollmentsService;
        }

        public void LoadComparisonData(AllDistrictComparisonsDto response)
        {
            var expendituresByDistrict = BudgetExpendituresService.GetAll().ToDictionary(x => x.DistrictId);
            var enrollmentsByDistrict = EnrollmentsService.GetAll().ToDictionary(x => x.DistrictId);
            BudgetsService.Prime();

            foreach (var districtContainer in response.DistrictFiscalYearMetrics)
            {
                //if we don't have expenditures for this district... pass
                if(!expendituresByDistrict.ContainsKey(districtContainer.District.DistrictId))
                    continue;

                //if we don't have enrollment data for this district... pass
                if(!enrollmentsByDistrict.ContainsKey(districtContainer.District.DistrictId))
                    continue;
 
                //get expenditures for this district
                var districtExpenditures = expendituresByDistrict[districtContainer.District.DistrictId];
                var fiscalYearExpenditures = districtExpenditures.FiscalYearAmounts.ToDictionary(x => x.FiscalYearId);

                //for each fiscal year
                foreach (var fyMetric in districtContainer.MetricsByFiscalYear)
                {
                    //if we don't have expenditures for this year... pass
                    if(!fiscalYearExpenditures.ContainsKey(fyMetric.Key))
                        continue;
 
                    //calculate the expenditure per student based on average enrollment...
                    var fyExpenditures = (decimal) fiscalYearExpenditures[fyMetric.Key].Total;
                    var enrollment = (decimal) enrollmentsByDistrict[districtContainer.District.DistrictId].Enrollment;
                    var costPerStudent = fyExpenditures / enrollment;

                    //add the metric
                    fyMetric.Value.Metrics.Add(ComparisonType.TotalCost, fyExpenditures);
                    fyMetric.Value.Metrics.Add(ComparisonType.TotalCostPerStudent, costPerStudent);

                    //now make comparisons to previous year
                    var budget = BudgetsService.Find(districtContainer.District.DistrictId, fyMetric.Key);
                    var previousBudget = BudgetsService.FindPreviousYear(budget);

                    //only make comparison if we have data
                    if(previousBudget == null)
                        continue;

                    if(!fiscalYearExpenditures.ContainsKey(previousBudget.FiscalYearId))
                        continue;

                    var previousYearExpenditures = (decimal) fiscalYearExpenditures[previousBudget.FiscalYearId].Total;

                    var costIncrease = fyExpenditures - previousYearExpenditures;
                    var costPerStudentIncrease = costIncrease / enrollment;
                    

                    fyMetric.Value.Metrics.Add(ComparisonType.TotalCostIncrease, costIncrease);
                    fyMetric.Value.Metrics.Add(ComparisonType.TotalCostIncreasePerStudent, costPerStudentIncrease);
                }
            }
        }
    }
}
