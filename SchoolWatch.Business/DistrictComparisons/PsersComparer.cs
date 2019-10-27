using System.Linq;
using Microsoft.Extensions.Logging; 
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers; 
using SchoolWatch.Data.Repositories.Interfaces;

// ReSharper disable InconsistentNaming

namespace SchoolWatch.Business.DistrictComparisons
{
    public class PsersComparer : IDistrictComparer
    { 
        public const int EXPENDITURES_CODE_SALARY = 100;
        public const int REVENUES_CODE_STATE_PENSION_CONTRIBUTION = 7820;
        private readonly IStatePensionRatesRepository StatePensionRatesRepository;
        private readonly IBudgetRevenuesService RevenuesService;
        private readonly IBudgetExpendituresService ExpendituresService;
        private readonly ILogger<PsersComparer> Logger;

        public PsersComparer(
            IStatePensionRatesRepository statePensionRatesRepository,
            IBudgetRevenuesService revenuesService,
            IBudgetExpendituresService expendituresService,
            ILogger<PsersComparer> logger
        )
        {
            StatePensionRatesRepository = statePensionRatesRepository;
            RevenuesService = revenuesService;
            ExpendituresService = expendituresService;
            Logger = logger;
        }


        public void LoadComparisonData(AllDistrictComparisonsDto response)
        {
            var fyContributionRates = StatePensionRatesRepository.GetAll().ToDictionary(x => x.FiscalYearId);
            var stateContributionByDistrict = RevenuesService.GetAllForCode(REVENUES_CODE_STATE_PENSION_CONTRIBUTION).ToDictionary(x => x.DistrictId);
            var salariesByDistrict = ExpendituresService.GetAllForCode(EXPENDITURES_CODE_SALARY).ToDictionary(x => x.DistrictId);

            //for each district..
            foreach (var districtContainer in response.DistrictFiscalYearMetrics)
            { 
                //------------------ First check if we have salary and state funding data for the district ---------------------------

                //if we don't have revenues for this district... pass
                if (!stateContributionByDistrict.TryGetValue(districtContainer.District.DistrictId, out var districtStateContributions))
                {
                    Logger.LogDebug($"No state PSERS contribution revenue data for district '{districtContainer.District.Name}'");
                    continue;
                }

                //if we don't have salaries for this district... pass
                if(!salariesByDistrict.TryGetValue(districtContainer.District.DistrictId, out var districtSalaries))
                {
                    Logger.LogDebug($"No salary data for district '{districtContainer.District.Name}'");
                    continue;
                }

                var fiscalYearSalaries =  districtSalaries.FiscalYearAmounts.ToDictionary(x => x.FiscalYearId);
                var fiscalYearStateContributions = districtStateContributions.FiscalYearAmounts.ToDictionary(x => x.FiscalYearId);

                //for each fiscal year
                PsersComparisonCalculationValues previousFyCalculation = null;
                foreach (var fyMetric in districtContainer.MetricsByFiscalYear.OrderBy(x => x.Key))
                {

                    if (!fiscalYearSalaries.TryGetValue(fyMetric.Key, out var fySalaries))
                    {
                        Logger.LogDebug($"No salary data for fiscal year '{fyMetric.Key}' for district '{districtContainer.District.Name}'");
                        continue;
                    }

                    if (!fiscalYearStateContributions.TryGetValue(fyMetric.Key, out var fyStateContributions))
                    {
                        Logger.LogDebug($"No state contribution revenue data for fiscal year '{fyMetric.Key}' for district '{districtContainer.District.Name}'");
                        continue;
                    }

                    if (!fyContributionRates.TryGetValue(fyMetric.Key, out var fyPsersContributionRate))
                    {
                        Logger.LogDebug($"No state pension rate data for fiscal year '{fyMetric.Key}' for district '{districtContainer.District.Name}'");
                        continue;
                    }

                    var totalContribution = fySalaries.Total * fyPsersContributionRate.TotalContributionRate;
                    var netContribution = totalContribution - fyStateContributions.Total;

                    var currentFyCalculation = new PsersComparisonCalculationValues
                    {
                        Value = netContribution,

                        Salaries = fySalaries.Total,
                        StateContribution = fyStateContributions.Total,
                        TotalContributionRate = fyPsersContributionRate.TotalContributionRate,
                        TotalContribution = (int) totalContribution,
                        DistrictNetContribution = (int) netContribution
                    };

                    //add the metric
                    fyMetric.Value.Metrics.Add(ComparisonType.PsersNetContribution, netContribution);
                    fyMetric.Value.Metrics.Add(ComparisonType.PsersStateContribution, fyStateContributions.Total);
 
                    //attempt to do year-over-year calculations...
                    if (previousFyCalculation != null)
                    {
                        var netContributionIncrease = currentFyCalculation.DistrictNetContribution - previousFyCalculation.DistrictNetContribution;
                        fyMetric.Value.Metrics.Add(ComparisonType.PsersNetContributionIncrease, netContributionIncrease);

                        currentFyCalculation.DistrictNetContributionIncrease = netContributionIncrease;
                    }

                    //add all calculation values
                    fyMetric.Value.Metrics.Add(ComparisonType.PsersAll, currentFyCalculation);

                    previousFyCalculation = currentFyCalculation;
                }
            }
        }
    }

    public class PsersComparisonCalculationValues
    {

        public object Value { get; set; }

        /// <summary>
        /// Total Salaries
        /// </summary>
        public int Salaries { get; set; }

        /// <summary>
        /// Districts Net Contribution
        /// </summary>
        public int DistrictNetContribution { get; set; }

        public int DistrictNetContributionIncrease { get; set; }

        /// <summary>
        /// States Contribution
        /// </summary>
        public int StateContribution { get; set; }

        /// <summary>
        /// Total Contribution
        /// </summary>
        public int TotalContribution { get; set; }

        /// <summary>
        /// Net Contribute Rate District Is Paying
        /// </summary>
        public decimal NetContributionRate { get; set; }

        /// <summary>
        /// Total Contribution Rate
        /// </summary>
        public decimal TotalContributionRate { get; set; }
    }
}
