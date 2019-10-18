using System.Collections.Generic;

namespace SchoolWatch.Business.DTO
{
    public class AllExpenditureCodesDto
    {
        public Dictionary<int, BudgetCodeDto> TopLevelCodes { get; set; } 
        public Dictionary<int, BudgetCodeDto> MidLevelCodes { get; set; }
        public Dictionary<int, BudgetCodeDto> CodeLevelCodes { get; set; }
    }
}
