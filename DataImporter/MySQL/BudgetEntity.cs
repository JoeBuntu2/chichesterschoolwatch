namespace DataImporter.MySQL
{
    public class BudgetEntity
    {
        public int BudgetId { get; set; }
        public int DistrictId { get; set; }
        public int FiscalYearId { get; set; }

        public long? Assessed { get; set; }
        public decimal? Millage { get; set; }
        public int? Homestead { get; set; }
        public decimal? CollectionRate { get; set; }
        public int? TaxLevy { get; set; }
    }
}
