using SchoolWatch.Data.Entities;

namespace SchoolWatch.Business.Interface
{
    public interface IBudgetsService
    {
        void Prime();
        BudgetEntity Find(int districtId, int fiscalYearId);
        BudgetEntity FindPreviousYear(BudgetEntity currentBudget);
    }
}
