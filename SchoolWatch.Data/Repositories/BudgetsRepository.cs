using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class BudgetsRepository : IBudgetsRepository
    {
        private readonly DbContext DbContext;
        private BudgetEntity[] All;

        public BudgetsRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }
         
        public BudgetEntity[] GetYearsBetween(int fiscalYearStart, int fiscalYearEnd)
        {
            All = All ?? DbContext.Set<BudgetEntity>().Include(x => x.FiscalYear).ToArray();

            return All
                .Where(x => x.FiscalYear.Start >= fiscalYearStart)
                .Where(x => x.FiscalYear.End <= fiscalYearEnd)
                .ToArray();
        }
    }
}