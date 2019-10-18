using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers; 

namespace SchoolWatch.Business.DistrictComparisons
{
    /// <summary>
    /// The shit the board members know and love...
    /// </summary>
    public class TaxRateIncreaseComparer : IDistrictComparer
    {
        private readonly IBudgetsService BudgetsService; 

        public TaxRateIncreaseComparer(IBudgetsService budgetsService )
        {
            BudgetsService = budgetsService; 
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
                        continue;

                    if(!budget.Millage.HasValue || !previousBudget.Millage.HasValue)
                        continue;

                    var millageChange = budget.Millage - previousBudget.Millage;
                    var taxRateIncrease = millageChange / previousBudget.Millage;


                    //add the metric
                    fyMetric.Value.Metrics.Add(ComparisonType.TaxRateIncrease, taxRateIncrease);
                }
            }
        }
    }
}
