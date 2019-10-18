using CsvHelper;
using DataImporter.FileRecords;
using DataImporter.MySQL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 

namespace DataImporter.ImportTasks
{
    public class ImportBudgetRevenuesTask : ITask
    {
        private Dictionary<int, RevenueEntity> RevenuesLookup;
        private FiscalYearEntity[] FiscalYears;
        private Dictionary<int, BudgetEntity> BudgetByFiscalYear;

        public void Run(IServiceProvider serviceProvider)
        {
            //configure console logging
            var dbContext = serviceProvider.GetService<SchoolDbContext>();

            var district = ChooseDistrict(dbContext);
            BudgetByFiscalYear = GetBudgetByFiscalYearMapping(dbContext, district);
            FiscalYears = dbContext.Set<FiscalYearEntity>().ToArray();

            var records = ReadRevenueRecords(district.Name);

            var budgetRevenues = ConvertToBudgetRevenueRecords(records);
            dbContext.AddRange(budgetRevenues);

            dbContext.SaveChanges();
        }

        private List<BudgetRevenueEntity> ConvertToBudgetRevenueRecords(IEnumerable<RevenueFileRecord> fileRecords)
        {
            var budgetRevenueRecords = new List<BudgetRevenueEntity>();

            foreach(var fileRecord in fileRecords)
            {
                foreach(var fiscalYear in FiscalYears)
                {
                    var budgetRecord = ConvertToBudgetRecord(fileRecord, fiscalYear);
                    if (budgetRecord == null)
                        continue;
                    budgetRevenueRecords.Add(budgetRecord);
                }
            }

            return budgetRevenueRecords;
        }

        private BudgetRevenueEntity ConvertToBudgetRecord(RevenueFileRecord fileRecord, FiscalYearEntity fiscalYear)
        {
            var budget = GetBudget(fiscalYear);

            var budgetRevenue = new BudgetRevenueEntity();
            budgetRevenue.BudgetId = budget.BudgetId;
            budgetRevenue.RevenueId = fileRecord.RevenueId;

            //get property value using reflection
            var propertyName = fiscalYear.Name.Replace("-", "");
           var amountText = (string)  fileRecord.GetType().GetProperty(propertyName).GetMethod.Invoke(fileRecord, null);

            if (string.IsNullOrWhiteSpace(amountText))
                return null;

            var amount = int.Parse(amountText, System.Globalization.NumberStyles.AllowThousands);
            budgetRevenue.Amount = amount;

            return budgetRevenue;
            
        }

        private Dictionary<int, BudgetEntity> GetBudgetByFiscalYearMapping(SchoolDbContext dbContext, DistrictEntity district)
        {
           return  dbContext.Set<BudgetEntity>()
                .Where(x => x.DistrictId == district.DistrictId)
                .ToDictionary(x => x.FiscalYearId, x => x);
        }

        private BudgetEntity GetBudget(FiscalYearEntity fiscalYear)
        {
            return BudgetByFiscalYear[fiscalYear.FiscalYearId];
        }

        private DistrictEntity ChooseDistrict(SchoolDbContext dbContext)
        {
            var districts = dbContext.Set<DistrictEntity>().ToArray();
            while (true)
            {
                Console.WriteLine("Choose District");
                for (var index = 0; index < districts.Length; index++)
                {
                    Console.WriteLine($"{index} {districts[index].Name}");
                }
                var choice = Console.ReadLine();
                if (int.TryParse(choice, out var indexChosen) && indexChosen < districts.Length)
                {
                    var chosenDistrict = districts[indexChosen];
                    Console.Write($"Enter 'Y' to confirm district '{chosenDistrict.Name}': ");
                    if(Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase))                        
                        return districts[indexChosen];                 
                }
            } 
        }

        private List<FileRecords.RevenueFileRecord> ReadRevenueRecords(string districtName)
        {
            using (var fs = File.OpenRead($"./Imports/{districtName}_Revenues.csv"))
            {
                using (var textReader = new StreamReader(fs))
                {
                    using (var csv = new CsvReader(textReader))
                    {
   
                        csv.Read();
                        csv.ReadHeader();
                        return csv.GetRecords<RevenueFileRecord>().ToList();
                    }
                }
            }

        }
    }
}

