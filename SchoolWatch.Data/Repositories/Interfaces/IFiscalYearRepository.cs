using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data.Repositories.Interfaces
{
    public interface IFiscalYearRepository
    {
        FiscalYearEntity[] GetYearsBetween(int yearStart, int yearEnd);

    }
}
