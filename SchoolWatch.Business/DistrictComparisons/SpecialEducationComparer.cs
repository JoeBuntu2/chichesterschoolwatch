using System.Linq;
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers;

namespace SchoolWatch.Business.DistrictComparisons
{
    public class SpecialEducation1200Comparer : IDistrictComparer
    {
        private readonly IBudgetExpendituresService BudgetExpendituresService;

        public SpecialEducation1200Comparer(IBudgetExpendituresService budgetExpendituresService)
        {
            BudgetExpendituresService = budgetExpendituresService;
        }

        public void LoadComparisonData(AllDistrictComparisonsDto response)
        {
            var expendituresByDistrict = BudgetExpendituresService.GetAll().ToDictionary(x => x.DistrictId);

            foreach (var districtContainer in response.DistrictFiscalYearMetrics)
            {
                //if we don't have expenditures for this district... pass
                if(!expendituresByDistrict.ContainsKey(districtContainer.District.DistrictId))
                    continue;
 
                //get expenditures for this district
                var districtExpenditures = expendituresByDistrict[districtContainer.District.DistrictId];
                var topLevel = districtExpenditures.TopLevelExpenditures.FirstOrDefault(x => x.TopLevelId == 1000);
                var midLevel = topLevel?.MidLevelExpenditures.FirstOrDefault(x => x.MidLevelId == 1200);
                if(midLevel == null)
                    continue;

                
                var fiscalYearExpenditures = midLevel.FiscalYearAmounts.ToDictionary(x => x.FiscalYearId);

                //for each fiscal year
                foreach (var fyMetric in districtContainer.MetricsByFiscalYear)
                {
                    //if we don't have expenditures for this year... pass
                    if(!fiscalYearExpenditures.ContainsKey(fyMetric.Key))
                        continue;
 
                    //calculate the expenditure per student based on average enrollment...
                    var amount1200 = (decimal) fiscalYearExpenditures[fyMetric.Key].Total;
                    var total = (decimal) districtExpenditures.FiscalYearAmounts.FirstOrDefault(x => x.FiscalYearId == fyMetric.Key).Total;
                    var percent = amount1200 / total;
 
                    //add the metric
                    fyMetric.Value.Metrics.Add(ComparisonType.SpecialEducation1200PercentageCost, percent);
                }
            }
        }
    }
}
