using System.Collections.Generic;

namespace SchoolWatch.Business.DTO
{
    public class TopLevelExpendituresDto
    {
        public int TopLevelId { get; set; } 
        
        public List<MidLevelExpendituresDto> MidLevelExpenditures { get; set; }

        public List<FiscalYearAmount> FiscalYearAmounts { get; set; }
 
    }
}