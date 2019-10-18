using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business
{
    public class FiscalYearsService : IFiscalYearsService
    {
        private readonly IFiscalYearRepository FiscalYearRepository;
        private const int FiscalYearStart = 2015;
        private const int FiscalYearEnd = 2020;
        private FiscalYearEntity[] SupportedYears;

        public FiscalYearsService(IFiscalYearRepository fiscalYearRepository)
        {
            FiscalYearRepository = fiscalYearRepository;
        }

        public int GetMin()
        {
            return FiscalYearStart;
        }

        public int GetMax()
        {
            return FiscalYearEnd;
        }

        public FiscalYearEntity[] GetSupportedYears()
        {
            SupportedYears = SupportedYears ?? FiscalYearRepository.GetYearsBetween(FiscalYearStart, FiscalYearEnd);
            return SupportedYears;
        }
    }
}