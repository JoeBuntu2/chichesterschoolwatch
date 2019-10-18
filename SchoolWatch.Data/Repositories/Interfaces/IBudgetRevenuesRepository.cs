using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data.Repositories.Interfaces
{
    public interface IBudgetRevenuesRepository
    {
        BudgetRevenueEntity[] GetAll();
    }
}
