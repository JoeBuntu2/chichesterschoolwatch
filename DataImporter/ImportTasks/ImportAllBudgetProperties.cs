using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using DataImporter.FileRecords;
using DataImporter.MySQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataImporter.ImportTasks
{
    public class ImportAllBudgetProperties : ITask
    {
        private FiscalYearEntity[] FiscalYears;
        private DistrictEntity[] Districts;
        private BudgetEntity[] Budgets;
        private SchoolDbContext DbContext;

        public void Run(IServiceProvider serviceProvider)
        {
            //configure console logging
            DbContext = serviceProvider.GetService<SchoolDbContext>();

            FiscalYears = DbContext.Set<FiscalYearEntity>().ToArray();
            Districts = DbContext.Set<DistrictEntity>().ToArray();
            Budgets = DbContext.Set<BudgetEntity>().ToArray();

            var fileRecords = ReadRecordsFromFile();

            CreateOrUpdateEntities(fileRecords);
            
            Console.WriteLine("Enter 'y' to continue...");
            if (Console.ReadLine().StartsWith("y"))
            {
                Console.WriteLine("Proceeding...");
                DbContext.SaveChanges();
            } 
        }

        private void CreateOrUpdateEntities(IEnumerable<BudgetPropertiesFileRecord> fileRecords)
        { 
            foreach (var fiscalYear in FiscalYears)
            {
                foreach (var fileRecord in fileRecords)
                {
                    ConvertToTotalEnrollmentEntity(fileRecord, fiscalYear);
                }
            }

            var tracked = DbContext.ChangeTracker.Entries<BudgetEntity>().ToArray();
            var newEntries = tracked.Count(x => x.State == EntityState.Added);
            var modified = tracked.Count(x => x.State == EntityState.Modified);
            var unchanged = tracked.Count(x => x.State == EntityState.Unchanged);

            Console.WriteLine($"New: {newEntries}, Modified: {modified}, Unchanged: {unchanged}");
        }

        private void ConvertToTotalEnrollmentEntity(BudgetPropertiesFileRecord fileRecord, FiscalYearEntity fiscalYear)
        {
            var district = Districts.SingleOrDefault(x => x.Name.Equals(fileRecord.District, StringComparison.InvariantCultureIgnoreCase));
                
            //skip record if we can't find district
            if (district == null)
            {
                Console.WriteLine($"Skipping Record... Unable to find district '{fileRecord.District}'");
                return;
            }

            var budget = Budgets.SingleOrDefault(x => x.DistrictId == district.DistrictId && x.FiscalYearId == fiscalYear.FiscalYearId);
            var newBudget = false;
            if (budget == null)
            {
                newBudget = true;
                budget = new BudgetEntity {DistrictId = district.DistrictId, FiscalYearId = fiscalYear.FiscalYearId};
            } 
            //get property value using reflection
            var columnName = fiscalYear.Name.Replace("-", "");
            var rawValueText = (string)  fileRecord.GetType().GetProperty(columnName).GetMethod.Invoke(fileRecord, null);

            if (string.IsNullOrWhiteSpace(rawValueText))
                return;

            if (fileRecord.Attribute.Contains("Assessed", StringComparison.InvariantCultureIgnoreCase))
            { 
                if (long.TryParse(rawValueText, NumberStyles.Any, CultureInfo.CurrentCulture, out var assessed))
                    budget.Assessed = assessed;
                else 
                    Console.WriteLine($"Skipping Record... Unable to parse '{fileRecord.Attribute}' value '{rawValueText}'");
            }
            else if (fileRecord.Attribute.Contains("Homestead", StringComparison.InvariantCultureIgnoreCase))
            { 
                if (int.TryParse(rawValueText, NumberStyles.Any, CultureInfo.CurrentCulture, out var homestead))
                    budget.Homestead = homestead;
                else 
                    Console.WriteLine($"Skipping Record... Unable to parse '{fileRecord.Attribute}' value '{rawValueText}'");
            }
            else if (fileRecord.Attribute.Contains("Tax Levy", StringComparison.InvariantCultureIgnoreCase))
            { 
                if (int.TryParse(rawValueText, NumberStyles.Any, CultureInfo.CurrentCulture, out var taxLeavy))
                    budget.TaxLevy = taxLeavy;
                else 
                    Console.WriteLine($"Skipping Record... Unable to parse '{fileRecord.Attribute}' value '{rawValueText}'");
            }
            else if (fileRecord.Attribute.Contains("Collection", StringComparison.InvariantCultureIgnoreCase))
            {
                if (decimal.TryParse(rawValueText, NumberStyles.Any, CultureInfo.CurrentCulture, out var collectionRate))
                    budget.CollectionRate = collectionRate;
                else 
                    Console.WriteLine($"Skipping Record... Unable to parse '{fileRecord.Attribute}' value '{rawValueText}'");
            }
            else if (fileRecord.Attribute.Contains("Millage", StringComparison.InvariantCultureIgnoreCase))
            {
                if (decimal.TryParse(rawValueText, NumberStyles.Any, CultureInfo.CurrentCulture, out var millage))
                    budget.Millage = millage;
                else 
                    Console.WriteLine($"Skipping Record... Unable to parse '{fileRecord.Attribute}' value '{rawValueText}'");
            }

            if (newBudget)
            {
                Console.WriteLine("Adding New Budget");
                DbContext.Set<BudgetEntity>().Add(budget);
            } 
        }
 
        private List<BudgetPropertiesFileRecord> ReadRecordsFromFile()
        {
            var totalsSkipped = 0;
            var blanksSkipped = 0;
            var records = new List<BudgetPropertiesFileRecord>();
            using (var fs = File.OpenRead($"./Imports/All_BudgetProps.csv"))
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

                            if (firstField.Contains("District", StringComparison.InvariantCultureIgnoreCase))
                            {
                                blanksSkipped++;
                                continue;
                            }
                            var record = csv.GetRecord<BudgetPropertiesFileRecord>();
                            records.Add(record);
                        }
                    }
                }
            }

            Console.WriteLine($"BudgetProperties - Blank Records: {blanksSkipped}, Totals Skipped: {totalsSkipped}");
            return records;
        }

    }
}
