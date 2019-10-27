using System.Collections.Generic;
using SchoolWatch.Business.DTO.Revenue;

namespace SchoolWatch.Business.Interface
{
    public interface IBudgetRevenuesService
    {
        List<DistrictRevenuesDto> GetAll();
        List<DistrictRevenuesDto> GetAllForCode(int code);
    }
}
