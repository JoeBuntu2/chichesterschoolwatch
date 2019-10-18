using SchoolWatch.Data.Entities;

namespace SchoolWatch.Business.Interface
{
    public interface IFiscalYearsService
    {
        int GetMin();
        int GetMax();
        FiscalYearEntity[] GetSupportedYears();
    }
}
