using System.Linq; 
using Microsoft.EntityFrameworkCore;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class FiscalYearRepository : IFiscalYearRepository
    {
        private readonly DbContext DbContext;
        private FiscalYearEntity[] All;

        public FiscalYearRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public FiscalYearEntity[] GetYearsBetween(int yearStart, int yearEnd)
        {
            All = All ?? DbContext.Set<FiscalYearEntity>().ToArray();

            return All
                .Where(x => x.Start >= yearStart)
                .Where(x => x.End <= yearEnd)
                .ToArray();
        }
    }
}
