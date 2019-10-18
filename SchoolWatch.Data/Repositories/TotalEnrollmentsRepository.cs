using System.Linq;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class TotalEnrollmentsRepository : ITotalEnrollmentsRepository
    {
        private readonly SchoolDbContext DbContext;
        private TotalEnrollmentEntity[] All;

        public TotalEnrollmentsRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TotalEnrollmentEntity[] GetAll()
        {
            All = All ?? DbContext.Set<TotalEnrollmentEntity>().ToArray();
            return All.ToArray();
        }
    }
}