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
    public class ImportBudgetExpendituresTask : ITask
    {
        private FiscalYearEntity[] FiscalYears;
        private Dictionary<int, BudgetEntity> BudgetByFiscalYear;

        public void Run(IServiceProvider serviceProvider)
        {
            //configure console logging
            var dbContext = serviceProvider.GetService<SchoolDbContext>();

            var district = ChooseDistrict(dbContext);
            BudgetByFiscalYear = GetBudgetByFiscalYearMapping(dbContext, district);
            FiscalYears = dbContext.Set<FiscalYearEntity>().ToArray();

            var records = ReadExpenditureRecords(district);
            
            var budgetExpenditures = ConvertToBudgetExpenditureRecords(records, FiscalYears);

            Console.Write($"Enter 'Y' to confirm {budgetExpenditures.Count} new expenditures for '{district.Name}': ");
            if (Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
                foreach(var expenditure in budgetExpenditures)
                {
                    dbContext.Add(expenditure);
                }
                

                dbContext.SaveChanges();
            }
        }


        private List<BudgetExpenditureEntity> ConvertToBudgetExpenditureRecords(IEnumerable<ExpenditureFileRecord> fileRecords, FiscalYearEntity[] fiscalYears)
        {
            var entities = new List<BudgetExpenditureEntity>();

            foreach(var fileRecord in fileRecords.Where(x => x.IsCode))
            {
                foreach (var fiscalYear in fiscalYears)
                {
                    var expenditure = ConvertToBudgetRecord(fileRecord, fiscalYear);
                    if(expenditure != null)
                        entities.Add(expenditure);
                }
            }

            return entities;
        }

        private BudgetExpenditureEntity ConvertToBudgetRecord(ExpenditureFileRecord fileRecord, FiscalYearEntity fiscalYear)
        {
            var budget = GetBudget(fiscalYear);

            var expenditure = new BudgetExpenditureEntity();
            expenditure.BudgetId = budget.BudgetId;
            expenditure.TopLevelId = fileRecord.Level1;
            expenditure.MidLevelId = fileRecord.Level2.Value;
            expenditure.CodeId = fileRecord.Code.Value;

            //get property value using reflection
            var propertyName = fiscalYear.Name.Replace("-", "");
            var amountText = (string)fileRecord.GetType().GetProperty(propertyName).GetMethod.Invoke(fileRecord, null);

            if (string.IsNullOrWhiteSpace(amountText))
                return null;

            var amount = int.Parse(amountText, System.Globalization.NumberStyles.AllowThousands);
            expenditure.Amount = amount;

            return expenditure;

        }

        private List<FileRecords.ExpenditureFileRecord> ReadExpenditureRecords(DistrictEntity district)
        {
            var totalsSkipped = 0;
            var blanksSkipped = 0;
            var records = new List<ExpenditureFileRecord>();
            using (var fs = File.OpenRead($"./Imports/{district.Name}_Expenditures.csv"))
            {
                using (var textReader = new StreamReader(fs))
                {
                    using (var csv = new CsvReader(textReader))
                    {

                        csv.Read();
                        csv.ReadHeader();

                        while (csv.Read())
                        {
                            //skip any totals or empty records
                            var firstField = csv.GetField(0);
                            if (string.IsNullOrWhiteSpace(firstField))
                            {
                                blanksSkipped++;
                                continue;
                            }
                            if (firstField.StartsWith("Total", StringComparison.InvariantCultureIgnoreCase))
                            {
                                totalsSkipped++;
                                continue;
                            }

                            var record = csv.GetRecord<ExpenditureFileRecord>();
                            records.Add(record);
                        }
                    }
                }
            }

            Console.WriteLine($"Expenditures - Blank Records: {blanksSkipped}, Totals Skipped: {totalsSkipped}");
            return records;
        }

        private Dictionary<int, BudgetEntity> GetBudgetByFiscalYearMapping(SchoolDbContext dbContext, DistrictEntity district)
        {
            return dbContext.Set<BudgetEntity>()
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
                    if (Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase))
                        return districts[indexChosen];
                }
            }
        }
    }
}
