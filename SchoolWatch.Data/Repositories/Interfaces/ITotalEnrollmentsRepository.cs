using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data.Repositories.Interfaces
{
    public interface ITotalEnrollmentsRepository
    {
        TotalEnrollmentEntity[] GetAll();
    }
}
