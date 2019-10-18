using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data.Repositories.Interfaces
{
    public interface IBudgetsRepository
    {
        BudgetEntity[] GetYearsBetween(int yearStart, int yearEnd);

    }
}
