using System.Linq;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business.Interface
{
    public class BudgetsService : IBudgetsService
    {
        private readonly IBudgetsRepository BudgetsRepository;
        private readonly IFiscalYearsService FiscalYearsService;

        public BudgetsService(IBudgetsRepository budgetsRepository, IFiscalYearsService fiscalYearsService)
        {
            BudgetsRepository = budgetsRepository;
            FiscalYearsService = fiscalYearsService;
        }

        // ReSharper disable once InconsistentNaming
        private BudgetEntity[] _All;
        private BudgetEntity[] All
        {
            get
            {
                _All = _All ?? BudgetsRepository.GetYearsBetween(FiscalYearsService.GetMin(), FiscalYearsService.GetMax());
                return _All;
            }
        }
 
        public BudgetEntity FindByBudgetId(int budgetId)
        {
            return All.FirstOrDefault(x => x.BudgetId == budgetId);
        } 

        public BudgetEntity Find(int districtId, int fiscalYearId)
        {
            return All.SingleOrDefault(x => x.DistrictId == districtId && x.FiscalYearId == fiscalYearId);
        }

        public BudgetEntity FindPreviousYear(BudgetEntity currentBudget)
        {
            if (currentBudget == null)
                return null;

            var fiscalYears = FiscalYearsService.GetSupportedYears();
            
            var currentYear = fiscalYears.FirstOrDefault(x => x.FiscalYearId == currentBudget.FiscalYearId);
            if (currentYear == null)
                return null;

            var previous = fiscalYears.FirstOrDefault(x => x.Start == currentYear.Start - 1);
            if (previous == null)
                return null;

            return All.FirstOrDefault(x => x.DistrictId == currentBudget.DistrictId && x.FiscalYearId == previous.FiscalYearId);
 
        }
    }
}