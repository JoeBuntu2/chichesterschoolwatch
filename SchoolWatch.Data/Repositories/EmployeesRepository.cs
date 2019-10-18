using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly DbContext DbContext;

        public EmployeesRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public EmployeeEntity[] GetAll()
        {
            return DbContext.Set<EmployeeEntity>()
                .OrderBy(x => x.Position)
                .ThenByDescending(x => x.Salary) 
                .ToArray();
        }
    }
}
