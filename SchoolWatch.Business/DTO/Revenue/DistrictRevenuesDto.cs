using System.Collections.Generic; 

namespace SchoolWatch.Business.DTO.Revenue
{
    public class DistrictRevenuesDto
    {
        public int DistrictId { get; set; }
        public string Name { get; set; }

        public List<SourceRevenuesDto> Sources { get; set; }

        public List<FiscalYearAmount> FiscalYearAmounts { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
