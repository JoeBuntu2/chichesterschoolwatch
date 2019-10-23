using SchoolWatch.Data.Entities;

namespace SchoolWatch.Business.Interface
{
    public interface IBudgetsService
    { 
        BudgetEntity Find(int districtId, int fiscalYearId);
        BudgetEntity FindPreviousYear(BudgetEntity currentBudget);
        BudgetEntity FindByBudgetId(int budgetId);
    }
}
