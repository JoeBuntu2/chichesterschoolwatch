using SchoolWatch.Data.Entities;

namespace SchoolWatch.Data.Repositories.Interfaces
{
    public interface IDistrictsRepository
    {
        DistrictEntity[] GetAll();
        DistrictEntity Get(int districtId);
    }
}
