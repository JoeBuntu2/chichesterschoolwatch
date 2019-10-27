using System.Linq;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class StatePensionRatesRepository : IStatePensionRatesRepository
    {
        private readonly SchoolDbContext DbContext;
        private StatePensionRateEntity[] All;

        public StatePensionRatesRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public StatePensionRateEntity[] GetAll()
        {
            All = All ?? DbContext.Set<StatePensionRateEntity>().ToArray();
            return All.ToArray();
        }
    }
}