using System.Collections.Generic;
using System.Linq;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business
{
    public class BudgetExpendituresService : IBudgetExpendituresService
    {
        private const int FiscalYearStart = 2015;
        private const int FiscalYearEnd = 2020;
        private readonly IBudgetExpendituresRepository BudgetExpendituresRepository;
        private readonly IFiscalYearRepository FiscalYearRepository;
        private readonly IBudgetsRepository BudgetsRepository;


        public BudgetExpendituresService(
            IBudgetExpendituresRepository budgetExpendituresRepository,
            IFiscalYearRepository fiscalYearRepository,
            IBudgetsRepository budgetsRepository)
        {
            BudgetExpendituresRepository = budgetExpendituresRepository;
            FiscalYearRepository = fiscalYearRepository;
            BudgetsRepository = budgetsRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DistrictExpendituresDto> GetAll()
        { 
            //very tedious way of writing a sql pivot in C#

            var fiscalYears = FiscalYearRepository.GetYearsBetween(FiscalYearStart, FiscalYearEnd);
            var budgetsLookup = BudgetsRepository.GetYearsBetween(FiscalYearStart, FiscalYearEnd).ToDictionary(x => x.BudgetId);

            var districtExpenditures = new List<DistrictExpendituresDto>();

            var allExpenditures = BudgetExpendituresRepository.GetAll();
            foreach (var districtGroup in allExpenditures.GroupBy(x => budgetsLookup[x.BudgetId].DistrictId).OrderBy(x => x.Key))
            {
                districtExpenditures.Add(GenerateYearlyDistrictExpenditures(
                    districtGroup.Key, 
                    districtGroup.ToArray(),
                    budgetsLookup, fiscalYears
                )); 
            }

            return districtExpenditures;
        }

        /// <summary>
        /// Generates DTO that represents district expense containing all top-level codes and totals for each fiscal year
        /// </summary>
        /// <param name="districtId"></param>
        /// <param name="districtExpenditures"></param>
        /// <param name="budgetsLookup"></param>
        /// <param name="fiscalYears"></param>
        /// <returns></returns>
        internal virtual DistrictExpendituresDto GenerateYearlyDistrictExpenditures(
            int districtId,
            BudgetExpenditureEntity[] districtExpenditures,
            Dictionary<int, BudgetEntity> budgetsLookup,
            FiscalYearEntity[] fiscalYears
        )
        {
            var yearlyDistrictExpenditures = new DistrictExpendituresDto
            {
                DistrictId = districtId,
                TopLevelExpenditures = new List<TopLevelExpendituresDto>(),
                FiscalYearAmounts = new List<FiscalYearAmount>()
            };

            foreach (var codeGroup in districtExpenditures.GroupBy(x => x.TopLevelId).OrderBy(x => x.Key))
            {
                yearlyDistrictExpenditures.TopLevelExpenditures.Add(GenerateYearlyTopLevelExpenditures(
                    codeGroup.Key,
                    codeGroup.ToArray(),
                    budgetsLookup,
                    fiscalYears
                ));
            }

            //aggregate the code amounts for each fiscal year
            foreach (var fiscalYear in fiscalYears)
            {
                yearlyDistrictExpenditures.FiscalYearAmounts.Add(new FiscalYearAmount
                {
                    FiscalYear = fiscalYear.Name,
                    FiscalYearId = fiscalYear.FiscalYearId,
                    Total = yearlyDistrictExpenditures.TopLevelExpenditures.SelectMany(
                        x => x.FiscalYearAmounts.Where(fy => fy.FiscalYearId == fiscalYear.FiscalYearId)
                    ).Sum(x => x.Total)
                });
            }

            return yearlyDistrictExpenditures;
        }

        /// <summary>
        /// Creates DTO that represents top level expense containing all mid-level codes and totals for each fiscal year
        /// </summary> 
        internal virtual TopLevelExpendituresDto GenerateYearlyTopLevelExpenditures(
            int topLevelId,
            BudgetExpenditureEntity[] topLevelExpenditures,
            Dictionary<int, BudgetEntity> budgetsLookup,
            FiscalYearEntity[] fiscalYears
        )
        {
            var yearlyExpendituresForTopLevel = new TopLevelExpendituresDto
            {
                TopLevelId = topLevelId,
                MidLevelExpenditures = new List<MidLevelExpendituresDto>(),
                FiscalYearAmounts = new List<FiscalYearAmount>()
            };

            foreach (var codeGroup in topLevelExpenditures.GroupBy(x => x.MidLevelId).OrderBy(x => x.Key))
            {
                yearlyExpendituresForTopLevel.MidLevelExpenditures.Add(GenerateYearlyMidLevelExpenditures(
                    codeGroup.Key,
                    codeGroup.ToArray(),
                    budgetsLookup,
                    fiscalYears
                ));
            }

            //aggregate the code amounts for each fiscal year
            foreach (var fiscalYear in fiscalYears)
            {
                yearlyExpendituresForTopLevel.FiscalYearAmounts.Add(new FiscalYearAmount
                {
                    FiscalYear = fiscalYear.Name,
                    FiscalYearId = fiscalYear.FiscalYearId,
                    Total = yearlyExpendituresForTopLevel.MidLevelExpenditures.SelectMany(
                        x => x.FiscalYearAmounts.Where(fy => fy.FiscalYearId == fiscalYear.FiscalYearId)
                    ).Sum(x => x.Total)
                });
            }

            return yearlyExpendituresForTopLevel;
        }

        /// <summary>
        /// Creates DTO that represents mid level expense containing all sub-codes and totals for each fiscal year
        /// </summary> 
        internal virtual MidLevelExpendituresDto GenerateYearlyMidLevelExpenditures(
            int middleLevelId,
            BudgetExpenditureEntity[] yearlyMidLevelExpenditures,
            Dictionary<int, BudgetEntity> budgetsLookup,
            FiscalYearEntity[] fiscalYears
        )
        {
            var yearlyExpendituresForMidLevel = new MidLevelExpendituresDto
            {
                MidLevelId = middleLevelId,
                CodeExpenditures = new List<CodeExpendituresDto>(),
                FiscalYearAmounts = new List<FiscalYearAmount>()
            };

            foreach (var codeGroup in yearlyMidLevelExpenditures.GroupBy(x => x.CodeId).OrderBy(x => x.Key))
            {
                yearlyExpendituresForMidLevel.CodeExpenditures.Add(GenerateYearlyCodeExpenditures(
                    codeGroup.Key,
                    codeGroup.ToArray(),
                    budgetsLookup,
                    fiscalYears
                ));
            }

            //aggregate the code amounts for each fiscal year
            foreach (var fiscalYear in fiscalYears)
            {
                yearlyExpendituresForMidLevel.FiscalYearAmounts.Add(new FiscalYearAmount
                {
                    FiscalYear = fiscalYear.Name,
                    FiscalYearId = fiscalYear.FiscalYearId,
                    Total = yearlyExpendituresForMidLevel.CodeExpenditures.SelectMany(
                        x => x.FiscalYearAmounts.Where(fy => fy.FiscalYearId == fiscalYear.FiscalYearId)
                    ).Sum(x => x.Total)
                });
            }

            return yearlyExpendituresForMidLevel;
        }

        /// <summary>
        /// Generates DTO that represents code level expense containing totals for each fiscal year
        /// </summary>  
        internal virtual CodeExpendituresDto GenerateYearlyCodeExpenditures(
            int code,
            BudgetExpenditureEntity[] yearlyCodeExpenditures,
            Dictionary<int, BudgetEntity> budgetsLookup,
            FiscalYearEntity[] fiscalYears
        )
        {
            var codeExpenditure = new CodeExpendituresDto {CodeLevelId = code, FiscalYearAmounts = new List<FiscalYearAmount>()};

            foreach (var fiscalYear in fiscalYears)
            {
                //find code for each fiscal year (may not exist)
                var codeForFiscalYear = yearlyCodeExpenditures.FirstOrDefault(x => budgetsLookup[x.BudgetId].FiscalYearId == fiscalYear.FiscalYearId);

                codeExpenditure.FiscalYearAmounts.Add(new FiscalYearAmount
                {
                    FiscalYear = fiscalYear.Name,
                    FiscalYearId = fiscalYear.FiscalYearId,
                    Total = codeForFiscalYear?.Amount ?? 0
                });
            }

            return codeExpenditure;
        }
         
    }
}