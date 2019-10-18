using System.Collections.Generic;

namespace SchoolWatch.Business.DTO.Revenue
{
    public class SourceRevenuesDto
    { 
        public string LevelId { get; set; }
        public string LevelName { get; set; } 

        public List<RevenueFunctionDto> Functions { get; set; }
        public List<FiscalYearAmount> FiscalYearAmounts { get; set; }
    }
}
