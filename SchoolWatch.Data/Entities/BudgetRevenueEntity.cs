namespace SchoolWatch.Data.Entities
{
    public class BudgetRevenueEntity
    { 
        public int BudgetRevenueId { get; set; }
        public int BudgetId { get; set; }
        public int RevenueId { get; set; }
        public int Amount { get; set; }
    }
}
