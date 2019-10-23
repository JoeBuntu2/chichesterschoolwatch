using System.Collections.Generic;
using SchoolWatch.Business.Interface;
using SchoolWatch.Data.Repositories.Interfaces;

namespace SchoolWatch.Business.Something
{
    public class SomeNewService
    {
        private readonly IFiscalYearsService FiscalYearsService;
        private readonly IBudgetExpendituresRepository BudgetExpendituresRepository;

        public SomeNewService(IFiscalYearsService fiscalYearsService, IBudgetExpendituresRepository budgetExpendituresRepository)
        {
            FiscalYearsService = fiscalYearsService;
            BudgetExpendituresRepository = budgetExpendituresRepository;
        }

 

    }

    public class DistrictAmounts
    {
        public decimal ChichesterAmount { get; set; }
        public decimal ChichesterPercent { get; set; }
        public decimal PercentAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal TotalPerStudent { get; set; }
        public decimal AveragePerStudent { get; set; }
        public decimal ChichesterPerStudent { get; set; }
    }

}
