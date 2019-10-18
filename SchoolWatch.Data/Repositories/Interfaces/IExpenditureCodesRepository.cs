using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data.Repositories.Interfaces
{
    public interface IExpenditureCodesRepository
    {
        TopLevelExpenditureEntity[] GetTopLevelCodes();
        MidLevelExpenditureEntity[] GetMidLevelCodes();
        ExpenditureCodeEntity[] GetCodeLevelCodes();
    }
}
