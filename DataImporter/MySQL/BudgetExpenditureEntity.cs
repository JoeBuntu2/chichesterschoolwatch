namespace DataImporter.MySQL
{
    public class BudgetExpenditureEntity
    {
        public int ExpenditureId { get; set; }
        public int BudgetId { get; set; }
        public int TopLevelId { get; set; }
        public int MidLevelId { get; set; }
        public int CodeId { get; set; }
        public int Amount { get; set; }
    }
}
