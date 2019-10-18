using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class BudgetExpendituresRepository : IBudgetExpendituresRepository
    {
        private readonly DbContext DbContext;
        private   BudgetExpenditureEntity[] All;

        public BudgetExpendituresRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public BudgetExpenditureEntity[] GetAll()
        {
            All = All ?? DbContext.Set<BudgetExpenditureEntity>().ToArray();
            return All;
        }
    
    }
}