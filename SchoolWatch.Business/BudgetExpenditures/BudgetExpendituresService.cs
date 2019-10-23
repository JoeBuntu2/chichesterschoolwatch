using System.Collections.Generic;
using System.Linq;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business.BudgetExpenditures
{
    public class BudgetExpendituresService : IBudgetExpendituresService
    {
        private readonly IFiscalYearsService FiscalYearsService;
        private readonly IBudgetsService BudgetsService;
        private readonly IBudgetExpendituresRepository BudgetExpendituresRepository;


        public BudgetExpendituresService(
            IFiscalYearsService fiscalYearsService,
            IBudgetsService budgetsService,
            IBudgetExpendituresRepository budgetExpendituresRepository)
        {
            FiscalYearsService = fiscalYearsService;
            BudgetsService = budgetsService;
            BudgetExpendituresRepository = budgetExpendituresRepository;
        }

        public List<DistrictExpendituresDto> GetAll()
        {
            //very tedious way of writing a sql pivot in C#  
            var districtExpenditures = new List<DistrictExpendituresDto>();

            //group all expenditures by district
            var expendituresByDistrict = GetExpendituresGroupedByDistrict();

            //process each district expenditure group
            foreach (var districtGroup in expendituresByDistrict.OrderBy(x => x.Key))
            {
                districtExpenditures.Add(GenerateYearlyDistrictExpenditures(
                    districtGroup.Key,
                    districtGroup.ToArray()
                ));
            }

            return districtExpenditures;
        }



        internal virtual IEnumerable<IGrouping<int, BudgetExpenditureEntity>> GetExpendituresGroupedByDistrict()
        {
            var allExpenditures = BudgetExpendituresRepository.GetAll();
            return allExpenditures.GroupBy(x => BudgetsService.FindByBudgetId(x.BudgetId).DistrictId);
        }

        /// <summary>
        /// Generates DTO that represents district expense containing all top-level codes and totals for each fiscal year
        /// </summary> 
        internal virtual DistrictExpendituresDto GenerateYearlyDistrictExpenditures(
            int districtId,
            BudgetExpenditureEntity[] districtExpenditures
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
                    codeGroup.ToArray()
                ));
            }

            //aggregate the code amounts for each fiscal year 
            foreach (var fiscalYear in FiscalYearsService.GetSupportedYears())
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
            BudgetExpenditureEntity[] topLevelExpenditures
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
                    codeGroup.ToArray()
                ));
            }

            //aggregate the code amounts for each fiscal year
            foreach (var fiscalYear in FiscalYearsService.GetSupportedYears())
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
            BudgetExpenditureEntity[] yearlyMidLevelExpenditures
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
                    codeGroup.ToArray()
                ));
            }

            //aggregate the code amounts for each fiscal year
            foreach (var fiscalYear in FiscalYearsService.GetSupportedYears())
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
            BudgetExpenditureEntity[] yearlyCodeExpenditures
        )
        {
            var codeExpenditure = new CodeExpendituresDto {CodeLevelId = code, FiscalYearAmounts = new List<FiscalYearAmount>()};

            foreach (var fiscalYear in FiscalYearsService.GetSupportedYears())
            {
                //find code for each fiscal year (may not exist)
                var codeForFiscalYear = yearlyCodeExpenditures.FirstOrDefault(x => BudgetsService.FindByBudgetId(x.BudgetId).FiscalYearId == fiscalYear.FiscalYearId);
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