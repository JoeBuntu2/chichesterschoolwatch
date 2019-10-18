using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Data.Repositories
{
    public class ExpenditureCodesRepository : IExpenditureCodesRepository
    {
        private readonly DbContext DbContext;

        public ExpenditureCodesRepository(SchoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TopLevelExpenditureEntity[] GetTopLevelCodes()
        {
            return DbContext.Set<TopLevelExpenditureEntity>().ToArray();
        }

        public MidLevelExpenditureEntity[] GetMidLevelCodes()
        {
            return DbContext.Set<MidLevelExpenditureEntity>().ToArray();
        }

        public ExpenditureCodeEntity[] GetCodeLevelCodes()
        {
            return DbContext.Set<ExpenditureCodeEntity>().ToArray();
        }
    }
}