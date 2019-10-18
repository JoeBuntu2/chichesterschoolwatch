using System.Linq;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class DistrictsRepository : IDistrictsRepository
    {
        private readonly SchoolDbContext DbContext;
        private DistrictEntity[] All;

        public DistrictsRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public DistrictEntity[] GetAll()
        {
            All = All ?? DbContext.Set<DistrictEntity>().ToArray();

            return All;
        }

        public DistrictEntity Get(int districtId)
        {
            All = All ?? DbContext.Set<DistrictEntity>().ToArray();

            return All.FirstOrDefault(x => x.DistrictId == districtId);
        }
    }
}