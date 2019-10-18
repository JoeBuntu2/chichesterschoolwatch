using System.Collections.Generic;

namespace SchoolWatch.Business.DTO
{
    public class DistrictExpendituresDto
    {
        public int DistrictId { get; set; }
        public string Name { get; set; }

        public List<TopLevelExpendituresDto> TopLevelExpenditures { get; set; }

        public List<FiscalYearAmount> FiscalYearAmounts { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}