using System;
using System.Collections.Generic;
using System.Linq;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.DTO.Revenue;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business
{
    public class BudgetRevenuesService : IBudgetRevenuesService
    { 
        private const int FiscalYearStart = 2015;
        private const int FiscalYearEnd = 2020;
        private readonly IBudgetRevenuesRepository BudgetRevenuesRepository;
        private readonly IRevenuesRepository RevenuesRepository;
        private readonly IFiscalYearRepository FiscalYearRepository;
        private readonly IBudgetsRepository BudgetsRepository;
        private Dictionary<int, RevenueEntity> RevenuesLookup;
        private FiscalYearEntity[] FiscalYears;
        private Dictionary<int, BudgetEntity> BudgetsLookup;


        public BudgetRevenuesService(
            IBudgetRevenuesRepository budgetRevenuesRepository,
            IRevenuesRepository revenuesRepository,
            IFiscalYearRepository fiscalYearRepository,
            IBudgetsRepository budgetsRepository)
        {
            BudgetRevenuesRepository = budgetRevenuesRepository;
            RevenuesRepository = revenuesRepository;
            FiscalYearRepository = fiscalYearRepository;
            BudgetsRepository = budgetsRepository;
        }
         
        public List<DistrictRevenuesDto> GetAll()
        { 
            //very tedious way of writing a sql pivot in C#

            FiscalYears = FiscalYearRepository.GetYearsBetween(FiscalYearStart, FiscalYearEnd);
            BudgetsLookup = BudgetsRepository.GetYearsBetween(FiscalYearStart, FiscalYearEnd).ToDictionary(x => x.BudgetId);
            RevenuesLookup = RevenuesRepository.GetAll().ToDictionary(x => x.RevenueId);

            var districtExpenditures = new List<DistrictRevenuesDto>();

            var allRevenues = BudgetRevenuesRepository.GetAll();
            foreach (var districtGroup in allRevenues.GroupBy(x => BudgetsLookup[x.BudgetId].DistrictId).OrderBy(x => x.Key))
            {
                districtExpenditures.Add(GenerateYearlyDistrictRevenues(
                    districtGroup.Key, 
                    districtGroup.ToArray()
                )); 
            }

            return districtExpenditures;
        }
         
        internal virtual DistrictRevenuesDto GenerateYearlyDistrictRevenues(
            int districtId,
            BudgetRevenueEntity[] districtRevenues 
        )
        {
            var yearlyDistrictRevenues = new DistrictRevenuesDto()
            {
                DistrictId = districtId, 
                Sources = new List<SourceRevenuesDto>(),
                FiscalYearAmounts = new List<FiscalYearAmount>()
            };

            var levelCompararer = new LevelComparer();
            //group all district revenues by level/source
            foreach (var revenueSource in districtRevenues.GroupBy(x => RevenuesLookup[x.RevenueId].Level).OrderBy(x => x.Key, levelCompararer))
            {
                yearlyDistrictRevenues.Sources.Add(GenerateYearlySourceRevenues(
                    revenueSource.Key,
                    revenueSource.ToArray() 
                ));
            }

            //aggregate the code amounts for each fiscal year
            foreach (var fiscalYear in FiscalYears)
            {
                yearlyDistrictRevenues.FiscalYearAmounts.Add(new FiscalYearAmount
                {
                    FiscalYear = fiscalYear.Name,
                    FiscalYearId = fiscalYear.FiscalYearId,
                    Total = yearlyDistrictRevenues.Sources.SelectMany(
                        x => x.FiscalYearAmounts.Where(fy => fy.FiscalYearId == fiscalYear.FiscalYearId)
                    ).Sum(x => x.Total)
                });
            }

            return yearlyDistrictRevenues;
        }
 
        internal virtual SourceRevenuesDto GenerateYearlySourceRevenues(
            string level,
            BudgetRevenueEntity[] sourceRevenues 
        )
        {
            var yearlySourceRevenues = new SourceRevenuesDto
            {
                LevelId = level,
                Functions = new List<RevenueFunctionDto>(),
                FiscalYearAmounts = new List<FiscalYearAmount>()
            };

            foreach (var codeGroup in sourceRevenues.GroupBy(x => x.RevenueId).OrderBy(x => x.Key))
            {
                yearlySourceRevenues.Functions.Add(GenerateYearlyFunctionRevenues(
                    codeGroup.Key,
                    codeGroup.ToArray()
                ));
            }

            //aggregate the revenue amounts for each fiscal year
            foreach (var fiscalYear in FiscalYears)
            {
                yearlySourceRevenues.FiscalYearAmounts.Add(new FiscalYearAmount
                {
                    FiscalYear = fiscalYear.Name,
                    FiscalYearId = fiscalYear.FiscalYearId,
                    Total = yearlySourceRevenues.Functions.SelectMany(
                        x => x.FiscalYearAmounts.Where(fy => fy.FiscalYearId == fiscalYear.FiscalYearId)
                    ).Sum(x => x.Total)
                });
            }

            return yearlySourceRevenues;
        }
 

        /// <summary>
        /// Generates DTO that represents code level expense containing totals for each fiscal year
        /// </summary>  
        internal virtual RevenueFunctionDto GenerateYearlyFunctionRevenues(
            int revenueId,
            BudgetRevenueEntity[] yearlyFunctionRevenues
        )
        {
            var functionRevenue = new RevenueFunctionDto() { FunctionId = revenueId, FiscalYearAmounts = new List<FiscalYearAmount>()};

            foreach (var fiscalYear in FiscalYears)
            {
                //find code for each fiscal year (may not exist)
                var codeForFiscalYear = yearlyFunctionRevenues.FirstOrDefault(x => BudgetsLookup[x.BudgetId].FiscalYearId == fiscalYear.FiscalYearId);

                functionRevenue.FiscalYearAmounts.Add(new FiscalYearAmount
                {
                    FiscalYear = fiscalYear.Name,
                    FiscalYearId = fiscalYear.FiscalYearId,
                    Total = codeForFiscalYear?.Amount ?? 0
                });
            }

            return functionRevenue;
        }
    }

    public class LevelComparer : IComparer<string>
    {
        public Dictionary<string, string> Map = new Dictionary<string, string>
        {
            //MAP local/state/federal etc codes to numbers to make comparison possible
            {"L", "1"},
            {"S", "2"},
            {"F", "3"},
            {"O", "4"}
        };

        public int Compare(string x, string y)
        {
            return StringComparer.InvariantCulture.Compare(Map[x], Map[y]);
        }


    }
}
