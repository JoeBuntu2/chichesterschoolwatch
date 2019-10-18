using SchoolWatch.Business.DTO;

namespace SchoolWatch.Business.Interface
{
    public interface IExpenditureCodesService
    {
        AllExpenditureCodesDto GetAll();

        BudgetCodeDto[] GetTopLevelCodes();

        BudgetCodeDto[] GetMidLevelCodes();

        BudgetCodeDto[] GetCodeLevelCodes();
    }
}
