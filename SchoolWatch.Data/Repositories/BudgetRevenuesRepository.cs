using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class BudgetRevenuesRepository : IBudgetRevenuesRepository
    {
        private readonly DbContext DbContext;
        private BudgetRevenueEntity[] All;

        public BudgetRevenuesRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public BudgetRevenueEntity[] GetAll()
        {
            All = All ?? DbContext.Set<BudgetRevenueEntity>().ToArray();
            return All; 
        }
    
    }
}