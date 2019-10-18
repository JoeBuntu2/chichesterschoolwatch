using SchoolWatch.Business.DTO.DistrictComparisons;

namespace SchoolWatch.Business.Interface.DistrictComparers
{
    public interface IDistrictComparer
    {
        void LoadComparisonData(AllDistrictComparisonsDto allDistrictComparisons);
    }
}
