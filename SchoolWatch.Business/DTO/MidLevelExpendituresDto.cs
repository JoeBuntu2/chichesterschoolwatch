using System.Collections.Generic;

namespace SchoolWatch.Business.DTO
{
    public class MidLevelExpendituresDto
    {
        public int MidLevelId { get; set; } 

        public List<CodeExpendituresDto> CodeExpenditures { get; set; }

        public List<FiscalYearAmount> FiscalYearAmounts { get; set; }

        public override string ToString()
        {
            return $"MidLevel {MidLevelId}";
        }
    }
}