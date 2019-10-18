using System.Linq;
using SchoolWatch.Business.DTO;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Entities;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business
{
    public class EnrollmentsService : IEnrollmentsService
    {
        private TotalEnrollmentEntity[] All = null;
        private readonly ITotalEnrollmentsRepository TotalEnrollmentsRepository;

        public EnrollmentsService(ITotalEnrollmentsRepository totalEnrollmentsRepository)
        {
            TotalEnrollmentsRepository = totalEnrollmentsRepository;
        }

        public DistrictEnrollment[] GetAll()
        {
            All = All ?? TotalEnrollmentsRepository.GetAll();

            return All.GroupBy(x => x.DistrictId).Select(x =>
                new DistrictEnrollment
                {
                    DistrictId = x.Key,
                    Enrollment = (int) x.Average(g => g.Enrollment)
                }
            ).ToArray();
        }

        public DistrictEnrollment Find(int districtId, int fiscalYearId)
        {
            All = All ?? TotalEnrollmentsRepository.GetAll();

            return All
                .Where(x => x.DistrictId == districtId && x.FiscalYearId == fiscalYearId)
                .Select(x => new DistrictEnrollment
                    {
                        DistrictId = districtId,
                        Enrollment = x.Enrollment
                    }
                ).FirstOrDefault();
        }
    }
}