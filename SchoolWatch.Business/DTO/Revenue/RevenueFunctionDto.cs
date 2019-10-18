using System.Collections.Generic;

namespace SchoolWatch.Business.DTO.Revenue
{
    //represents an individual revenue function
    public class RevenueFunctionDto
    {
        public int FunctionId { get; set; }
        public string Description { get; set; }
        public List<FiscalYearAmount> FiscalYearAmounts { get; set; }
    }
}
