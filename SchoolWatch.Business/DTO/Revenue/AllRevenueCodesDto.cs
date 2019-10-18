using System.Collections.Generic; 

namespace SchoolWatch.Business.DTO.Revenue
{
    public class AllRevenueCodesDto
    {
        public Dictionary<string, string> Levels { get; set; }
        public Dictionary<int, RevenueCodeDto> FunctionCodes { get; set; }
    }
}
