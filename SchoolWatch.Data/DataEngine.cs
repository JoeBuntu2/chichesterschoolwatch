using Microsoft.EntityFrameworkCore;

namespace SchoolWatch.Data
{
    public class DataEngine : IDataEngine
    {
        private readonly SchoolDbContext DbContext;

        public DataEngine(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public void KeepAlive()
        {
            DbContext.Database.ExecuteSqlCommand("Select 1;");
        }
    }
}