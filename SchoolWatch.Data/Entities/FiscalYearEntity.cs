using System.Collections.Generic;

namespace SchoolWatch.Data.Entities
{
    public class FiscalYearEntity
    {
        public int FiscalYearId { get; set; }
        public int Start { get; set; }
        public string Name { get; set; }
        public int End { get; set; }
        
        public ICollection<BudgetEntity> Budgets { get; set; }
    }
}
