using System.Linq;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business.Interface
{
    public class BudgetsService : IBudgetsService
    {
        private readonly IBudgetsRepository BudgetsRepository;
        private readonly IFiscalYearsService FiscalYearsService;
        private BudgetEntity[] All;
        private FiscalYearEntity[] FiscalYears;

        public BudgetsService(IBudgetsRepository budgetsRepository, IFiscalYearsService fiscalYearsService)
        {
            BudgetsRepository = budgetsRepository;
            FiscalYearsService = fiscalYearsService;
        }
 
        public void Prime()
        {
            All = All ?? BudgetsRepository.GetYearsBetween(FiscalYearsService.GetMin(), FiscalYearsService.GetMax());
            FiscalYears = FiscalYearsService.GetSupportedYears();
        }

        public BudgetEntity Find(int districtId, int fiscalYearId)
        {
            return All.SingleOrDefault(x => x.DistrictId == districtId && x.FiscalYearId == fiscalYearId);
        }

        public BudgetEntity FindPreviousYear(BudgetEntity currentBudget)
        {
            if (currentBudget == null)
                return null;

            var currentYear = FiscalYears.FirstOrDefault(x => x.FiscalYearId == currentBudget.FiscalYearId);
            if (currentYear == null)
                return null;

            var previous = FiscalYears.FirstOrDefault(x => x.Start == currentYear.Start - 1);
            if (previous == null)
                return null;

            return All.FirstOrDefault(x => x.DistrictId == currentBudget.DistrictId && x.FiscalYearId == previous.FiscalYearId);


        }
    }
}