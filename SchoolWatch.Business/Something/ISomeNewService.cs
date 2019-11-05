using System.Collections.Generic;

namespace SchoolWatch.Business.Something
{
    public interface ISomeNewService
    {
        List<DistrictAmounts> CompareTwoBudgets(string firstDistrictName, string secondDistrictName);
    }
}