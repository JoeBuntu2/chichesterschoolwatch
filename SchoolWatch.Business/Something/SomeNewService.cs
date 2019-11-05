using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.Interface; 

namespace SchoolWatch.Business.Something
{
    public class SomeNewService : ISomeNewService
    {
        private readonly IFiscalYearsService FiscalYearsService;
        private readonly IDistrictsService DistrictsService;
        private readonly IBudgetExpendituresService BudgetExpendituresService;
        private readonly IExpenditureCodesService ExpenditureCodesService;
        private readonly IEnrollmentsService EnrollmentsService;
        private readonly ILogger<SomeNewService> Logger;

        public SomeNewService(
            IFiscalYearsService fiscalYearsService, 
            IDistrictsService districtsService,
            IBudgetExpendituresService budgetExpendituresService,
            IExpenditureCodesService expenditureCodesService,
            IEnrollmentsService enrollmentsService,
            ILogger<SomeNewService> logger)
        {
            FiscalYearsService = fiscalYearsService;
            DistrictsService = districtsService;
            BudgetExpendituresService = budgetExpendituresService;
            ExpenditureCodesService = expenditureCodesService;
            EnrollmentsService = enrollmentsService;

            Logger = logger;
        }

        private DistrictsDto District1 { get; set; }
        private DistrictsDto District2 { get; set; }

        private   DistrictExpendituresDto FirstDistrictExpenditures { get; set; }
        private  DistrictExpendituresDto SecondDistrictExpenditures { get; set; }
        private decimal TotalCostPerStudentDifference { get; set; }
        private const int latestYear = 4;
        private AllExpenditureCodesDto AllExpenditureCodes { get; set; }
        private  List<DistrictAmounts> ComparisonResults { get; set; }
        private  DistrictEnrollment District1Enrollment { get; set; }


        public List<DistrictAmounts> CompareTwoBudgets(string firstDistrictName, string secondDistrictName)
        {  
            ComparisonResults = new List<DistrictAmounts>();
 
            var districts = DistrictsService.GetAll();
            District1 = districts.FirstOrDefault(x => x.Name.Equals(firstDistrictName, StringComparison.InvariantCultureIgnoreCase));
            District2 = districts.FirstOrDefault(x => x.Name.Equals(secondDistrictName, StringComparison.InvariantCultureIgnoreCase));
 
            //if we cant find the districts, can't do anything
            if (District1 == null || District2 == null)
                return null;

            District1Enrollment = EnrollmentsService.GetAll().SingleOrDefault(x => x.DistrictId == District1.DistrictId);

            //find expenditures for each district
            var expenditures = BudgetExpendituresService.GetAll(); 
            FirstDistrictExpenditures =  expenditures.FirstOrDefault(x => x.DistrictId == District1.DistrictId);
            SecondDistrictExpenditures = expenditures.FirstOrDefault(x => x.DistrictId == District2.DistrictId);
 
            //if we don't have expenditures, can't do anything
            if(FirstDistrictExpenditures == null || SecondDistrictExpenditures == null)
                return null;

            //what is the total cost difference?
            TotalCostPerStudentDifference = FirstDistrictExpenditures.FiscalYearAmounts[latestYear].PerStudent -
                                  SecondDistrictExpenditures.FiscalYearAmounts[latestYear].PerStudent;


            AllExpenditureCodes = ExpenditureCodesService.GetAll();

         
            foreach(var top in AllExpenditureCodes.TopLevelCodes.OrderBy(x => x.Key))
            {
                AddTopLevelAmounts(top.Key);
            }

            return ComparisonResults;
        }

        private void AddTopLevelAmounts(int topLevelId)
        {
            var d1Tops = FirstDistrictExpenditures.TopLevelExpenditures.ToDictionary(x => x.TopLevelId);
            var d2Tops = SecondDistrictExpenditures.TopLevelExpenditures.ToDictionary(x => x.TopLevelId);
 
            if (!d1Tops.TryGetValue(topLevelId, out var d1TopLevelExpenditure))
            {
                Logger.LogDebug($"District '{District1.Name}' has no value for topLevel {topLevelId}");
                return;
            }

            if (!d2Tops.TryGetValue(topLevelId, out var d2TopLevelExpenditure))
            {
                Logger.LogDebug($"District '{District2.Name}' has no value for topLevel {topLevelId}");
                return;
            }

            var districtAmounts = new DistrictAmounts
            {
                TopId = topLevelId,
                District1Amount = d1TopLevelExpenditure.FiscalYearAmounts[latestYear].Total,
                District1PerStudent = d1TopLevelExpenditure.FiscalYearAmounts[latestYear].PerStudent,
                District2Amount = d2TopLevelExpenditure.FiscalYearAmounts[latestYear].Total,
                District2PerStudent = d2TopLevelExpenditure.FiscalYearAmounts[latestYear].PerStudent,

            };
            districtAmounts.CostPerStudentDifference = districtAmounts.District1PerStudent - districtAmounts.District2PerStudent;
            districtAmounts.PercentDifference = districtAmounts.CostPerStudentDifference / TotalCostPerStudentDifference;
            var topLevelCode = AllExpenditureCodes.TopLevelCodes[topLevelId];
            districtAmounts.Name = topLevelCode.Name;

            districtAmounts.District1Enrollment = District1Enrollment.Enrollment;
            districtAmounts.TotalCostDifference = districtAmounts.CostPerStudentDifference * District1Enrollment.Enrollment;

            if(topLevelId == 5000)
                ComparisonResults.Add(districtAmounts);

            foreach (var midLevelCode in AllExpenditureCodes.MidLevelCodes)
            {
                AddMidLevelAmounts(topLevelId, midLevelCode.Key, d1TopLevelExpenditure, d2TopLevelExpenditure);
            } 
        }

        private void AddMidLevelAmounts(
            int topLevelId,
            int midLevelId, 
            TopLevelExpendituresDto district1TopLevel, 
            TopLevelExpendituresDto district2TopLevel)
        {
            var d1Mids = district1TopLevel.MidLevelExpenditures.ToDictionary(x => x.MidLevelId);
            var d2Mids = district2TopLevel.MidLevelExpenditures.ToDictionary(x => x.MidLevelId);
 
            if (!d1Mids.TryGetValue(midLevelId, out var d1MidLevelExpenditures))
            {
                Logger.LogDebug($"District '{District1.Name}' has no value for midLevel {midLevelId}");
                return;
            }

            if (!d2Mids.TryGetValue(midLevelId, out var d2MidLevelExpenditures))
            {
                Logger.LogDebug($"District '{District2.Name}' has no value for midLevel {midLevelId}");
                return;
            }

            var districtAmounts = new DistrictAmounts
            {
                TopId = district1TopLevel.TopLevelId,
                MidId = midLevelId,
                District1Amount = d1MidLevelExpenditures.FiscalYearAmounts[latestYear].Total,
                District1PerStudent = d1MidLevelExpenditures.FiscalYearAmounts[latestYear].PerStudent,
                District2Amount = d2MidLevelExpenditures.FiscalYearAmounts[latestYear].Total,
                District2PerStudent = d2MidLevelExpenditures.FiscalYearAmounts[latestYear].PerStudent,

            };
            var midLevelCode = AllExpenditureCodes.MidLevelCodes[midLevelId];
            districtAmounts.Name = midLevelCode.Name;
            districtAmounts.CostPerStudentDifference = districtAmounts.District1PerStudent - districtAmounts.District2PerStudent;
            districtAmounts.PercentDifference = districtAmounts.CostPerStudentDifference / TotalCostPerStudentDifference;
            districtAmounts.District1Enrollment = District1Enrollment.Enrollment;
            districtAmounts.TotalCostDifference = districtAmounts.CostPerStudentDifference * District1Enrollment.Enrollment;
            //ComparisonResults.Add(districtAmounts);

            foreach (var codeLevelCode in AllExpenditureCodes.CodeLevelCodes)
            {
                AdCodeLevelAmounts(topLevelId, midLevelId, codeLevelCode.Key, d1MidLevelExpenditures, d2MidLevelExpenditures);
            }
        }

        private void AdCodeLevelAmounts(
            int topLevelId,
            int midLevelId,
            int codeId, 
            MidLevelExpendituresDto district1MidLevel, 
            MidLevelExpendituresDto district2MidLevel)
        {
            var d1Codes = district1MidLevel.CodeExpenditures.ToDictionary(x => x.CodeLevelId);
            var d2Codes = district2MidLevel.CodeExpenditures.ToDictionary(x => x.CodeLevelId);
 
            if (!d1Codes.TryGetValue(codeId, out var d1CodeLevelExpenditures))
            {
                Logger.LogDebug($"District '{District1.Name}' has no value for codeId {codeId}");
                return;
            }

            if (!d2Codes.TryGetValue(codeId, out var d2CodeLevelExpenditures))
            {
                Logger.LogDebug($"District '{District2.Name}' has no value for codeId {codeId}");
                return;
            }

            var districtAmounts = new DistrictAmounts
            {
                TopId = topLevelId,
                MidId = midLevelId,
                CodeId = codeId,
                District1Amount = d1CodeLevelExpenditures.FiscalYearAmounts[latestYear].Total,
                District1PerStudent = d1CodeLevelExpenditures.FiscalYearAmounts[latestYear].PerStudent,
                District2Amount = d2CodeLevelExpenditures.FiscalYearAmounts[latestYear].Total,
                District2PerStudent = d2CodeLevelExpenditures.FiscalYearAmounts[latestYear].PerStudent,

            };
            var codeLevelCode = AllExpenditureCodes.CodeLevelCodes[codeId];
            districtAmounts.Name = codeLevelCode.Name;

            districtAmounts.CostPerStudentDifference = districtAmounts.District1PerStudent - districtAmounts.District2PerStudent;
            districtAmounts.District1Enrollment = District1Enrollment.Enrollment;
            districtAmounts.TotalCostDifference = districtAmounts.CostPerStudentDifference * District1Enrollment.Enrollment;
            districtAmounts.PercentDifference = districtAmounts.CostPerStudentDifference / TotalCostPerStudentDifference;
            
            if(topLevelId != 5000)
             ComparisonResults.Add(districtAmounts);
 
        }
    }

    

    public class DistrictAmounts
    {
        public int TopId { get; set; }
        public int? MidId { get; set; }
        public int? CodeId { get; set; }
        public string Name { get; set; }
        public int District1Enrollment { get; set; }

        public decimal District1Amount { get; set; }
        public decimal District1PerStudent { get; set; }

        public decimal District2Amount { get; set; }
        public decimal District2PerStudent { get; set; }

        public decimal CostPerStudentDifference { get; set; } 
        public decimal PercentDifference { get; set; }

        public decimal TotalCostDifference { get; set; }
    }

}
