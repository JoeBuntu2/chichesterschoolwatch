using System.Collections.Generic;
using SchoolWatch.Business.DTO;

namespace SchoolWatch.Business.Interface
{
    public interface IBudgetExpendituresService
    {
        List<DistrictExpendituresDto> GetAll();
    }
}
