using System.Collections.Generic;

namespace SchoolWatch.Business.DTO
{
    public class CodeExpendituresDto
    {
        public int CodeLevelId { get; set; }
        public List<FiscalYearAmount> FiscalYearAmounts { get; set; }
    }
}