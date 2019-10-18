using System;
using System.Linq;
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers;

namespace SchoolWatch.Business.DistrictComparisons
{
    /// <summary>
    /// Budget Deficit Comparisons
    /// </summary>
    public class DeficitComparer : IDistrictComparer
    {
        private readonly IBudgetExpendituresService BudgetExpendituresService;
        private readonly IBudgetRevenuesService BudgetRevenuesService;
        private readonly IEnrollmentsService EnrollmentsService;

        public DeficitComparer(
            IBudgetExpendituresService budgetExpendituresService, 
            IBudgetRevenuesService budgetRevenuesService,
            IEnrollmentsService enrollmentsService)
        {
            BudgetExpendituresService = budgetExpendituresService;
            BudgetRevenuesService = budgetRevenuesService;
            EnrollmentsService = enrollmentsService;
        }
        public void LoadComparisonData(AllDistrictComparisonsDto response)
        {
            var expendituresByDistrict = BudgetExpendituresService.GetAll().ToDictionary(x => x.DistrictId);
            var revenuesByDistrict = BudgetRevenuesService.GetAll().ToDictionary(x => x.DistrictId);
            var enrollmentsByDistrict = EnrollmentsService.GetAll().ToDictionary(x => x.DistrictId);

            foreach (var districtContainer in response.DistrictFiscalYearMetrics)
            {
                //if we don't have expenditures for this district... pass
                if(!expendituresByDistrict.ContainsKey(districtContainer.District.DistrictId))
                    continue;

                //if we don't have enrollment data for this district... pass
                if(!revenuesByDistrict.ContainsKey(districtContainer.District.DistrictId))
                    continue;
 
                //get expenditures for this district
                var districtExpenditures = expendituresByDistrict[districtContainer.District.DistrictId];
                var fiscalYearExpenditures = districtExpenditures.FiscalYearAmounts.ToDictionary(x => x.FiscalYearId);

                var districtRevenues = revenuesByDistrict[districtContainer.District.DistrictId];
                var fiscalYearRevenues = districtRevenues.FiscalYearAmounts.ToDictionary(x => x.FiscalYearId);

                //for each fiscal year
                foreach (var fyMetric in districtContainer.MetricsByFiscalYear)
                {
                    //if we don't have expenditures for this year... pass
                    if (!fiscalYearExpenditures.ContainsKey(fyMetric.Key))
                        continue;

                    //calculate the expenditure per student based on average enrollment...
                    var fyExpenditures = fiscalYearExpenditures[fyMetric.Key];
                    var fyRevenues = fiscalYearRevenues[fyMetric.Key];
                    decimal deficit = (fyRevenues.Total - fyExpenditures.Total);

                    //positive values are just confusing. normalize to zero.
                    if (deficit > 0)
                        deficit = 0;

                    //deficit should be positive value
                    deficit = Math.Abs(deficit);

                    var enrollment = (decimal) enrollmentsByDistrict[districtContainer.District.DistrictId].Enrollment;

                    //calculate deficit per student 
                    var deficitPerStudent = deficit / enrollment;


                    //add the metric
                    fyMetric.Value.Metrics.Add(ComparisonType.Deficit, deficit);
                    fyMetric.Value.Metrics.Add(ComparisonType.DeficitPerStudent, deficitPerStudent);
                }
            }
        }
    }
}
