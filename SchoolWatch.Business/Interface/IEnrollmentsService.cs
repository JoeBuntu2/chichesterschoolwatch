using SchoolWatch.Business.DTO;

namespace SchoolWatch.Business.Interface
{
    public interface IEnrollmentsService
    {
        DistrictEnrollment[] GetAll();
        DistrictEnrollment Find(int districtId, int fiscalYearId);
    }
}