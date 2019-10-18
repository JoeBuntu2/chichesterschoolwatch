using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class RevenuesRepository : IRevenuesRepository
    {
        private readonly DbContext DbContext;

        public RevenuesRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public RevenueEntity[] GetAll()
        {
            return DbContext.Set<RevenueEntity>().ToArray();
        }
    }
}