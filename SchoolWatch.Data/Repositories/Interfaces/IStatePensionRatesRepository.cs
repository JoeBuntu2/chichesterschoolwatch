using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data.Repositories.Interfaces
{
    interface IStatePensionRatesRepository
    {
        StatePensionRateEntity[] GetAll();
    }
}
