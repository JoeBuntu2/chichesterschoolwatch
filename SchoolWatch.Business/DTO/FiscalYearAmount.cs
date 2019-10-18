namespace SchoolWatch.Business.DTO
{
    public class FiscalYearAmount
    {
        public  int FiscalYearId { get; set; }
        public  string FiscalYear { get; set; }
        public  int Total { get; set; }

        public override string ToString()
        {
            return $"{FiscalYear} {Total:C0}";
        }
    }
}