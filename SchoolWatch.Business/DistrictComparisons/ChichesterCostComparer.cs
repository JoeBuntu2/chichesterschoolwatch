using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO.DistrictComparisons;
using SchoolWatch.Business.Interface;
using SchoolWatch.Business.Interface.DistrictComparers;

namespace SchoolWatch.Business.DistrictComparisons
{
    public class ChichesterCostComparer : IDistrictComparer
    {
        private readonly IBudgetsService BudgetsService;
        private readonly IEnrollmentsService EnrollmentsService;
        private readonly ILogger<ChichesterCostComparer> Logger;
        private const int ChichesterDistrictId = 4;

        public ChichesterCostComparer(
            IBudgetsService budgetsService, 
            IEnrollmentsService enrollmentsService, 
            ILogger<ChichesterCostComparer> logger)
        {
            BudgetsService = budgetsService;
            EnrollmentsService = enrollmentsService; 
            Logger = logger;
        }

        public void LoadComparisonData(AllDistrictComparisonsDto response)
        { 
            var enrollmentByDistrict = EnrollmentsService.GetAll().ToDictionary(x => x.DistrictId);
            var chichesterEnrollment = enrollmentByDistrict[ChichesterDistrictId]?.Enrollment;
            var chichesterContainer = response.DistrictFiscalYearMetrics.FirstOrDefault(x => x.District.DistrictId == ChichesterDistrictId);
            if(chichesterEnrollment == null || chichesterContainer == null)
                return;

            foreach (var districtContainer in response.DistrictFiscalYearMetrics)
            { 
                //for each fiscal year
                foreach (var fyMetric in districtContainer.MetricsByFiscalYear)
                { 
                    var budget = BudgetsService.Find(districtContainer.District.DistrictId, fyMetric.Key);
                    if (budget == null)
                        continue; 
 
                    var costPerStudent = (decimal?) fyMetric.Value.Metrics[ComparisonType.TotalCostPerStudent];
                    if (!costPerStudent.HasValue) 
                        return;

                    var chichesterMetrics = chichesterContainer.MetricsByFiscalYear[fyMetric.Key];
                    var chichesterCostPerStudent = (decimal?) chichesterMetrics.Metrics[ComparisonType.TotalCostPerStudent];
 
                    Logger.LogDebug($"Chichester {ComparisonType.TotalCostPerStudent}:{chichesterCostPerStudent}, {ComparisonType.Enrollment}:{chichesterEnrollment}");


                    //what is the difference in this districts cost-per-student than chichester?
                    var difference = Math.Abs(costPerStudent.Value - chichesterCostPerStudent.Value);
                     

                    //if chichester costs-per-student were the same as this district, how much would we save?
                    var chichesterExcessSpending =  difference * chichesterEnrollment;
 
                    fyMetric.Value.Metrics.Add(ComparisonType.CostPerStudentComparedToChichester, difference);


                    var data = new
                    { 
                        Value = chichesterExcessSpending,

                        //include all the intermediary values for proof
                        calculationValues = new {
                            ChichesterCostPerStudent = chichesterCostPerStudent,
                            DistrictCostPerStudent = costPerStudent.Value,
                            DifferenceCostPerStudent = difference,
                            ChichesterEnrollment = chichesterEnrollment, 
                        }
                    };

                    fyMetric.Value.Metrics.Add(ComparisonType.ExcessChichesterSpending, data);

                }
            }
        }
    }
}
