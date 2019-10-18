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
    /// <summary>
    /// Imports expenditure codes only
    /// </summary>
    public class ImportExpendituresTask : ITask
    {
        private Dictionary<int, TopLevelExpenditureEntity> TopLevels;
        private Dictionary<int, MidLevelExpenditureEntity> MidLevel;
        private Dictionary<int, ExpenditureCodeEntity> Codes;

        public void Run(IServiceProvider serviceProvider)
        {
            //configure console logging
            var dbContext = serviceProvider.GetService<SchoolDbContext>();
 
            TopLevels = dbContext.Set<TopLevelExpenditureEntity>().ToDictionary(x => x.TopLevelId, x => x);
            MidLevel = dbContext.Set<MidLevelExpenditureEntity>().ToDictionary(x => x.MidLevelId, x => x);
            Codes = dbContext.Set<ExpenditureCodeEntity>().ToDictionary(x => x.Code, x => x);

            var district = ChooseDistrict(dbContext);
            var records = ReadExpenditureRecords(district);

            var topLevelRecords = 0;
            var midLevelRecords = 0;
            var codeRecords = 0;

            foreach(var record in records)
            {
                if(record.IsTopLevel)
                {
                    if(!TopLevels.ContainsKey(record.Level1))
                    {
                        dbContext.Add(new TopLevelExpenditureEntity { TopLevelId = record.Level1, Description = record.Description });
                        topLevelRecords++;
                    }
                }
                else if(record.IsMidLevel)
                {
                    if(!MidLevel.ContainsKey(record.Level2.Value))
                    {
                        dbContext.Add(new MidLevelExpenditureEntity { MidLevelId = record.Level2.Value, Description = record.Description });
                        midLevelRecords++;
                    }
                }
                else if(record.IsCode)
                {
                    var codeEntity = new ExpenditureCodeEntity { Code = record.Code.Value, Description = record.Description };
                    if (dbContext.Set<ExpenditureCodeEntity>().Find(codeEntity.Code) == null)
                    {
                        dbContext.Add(codeEntity);
                        codeRecords++;
                    }                    
                }
                else
                {
                    throw new Exception();
                }
            }
            Console.WriteLine($"Records to import: Top: {topLevelRecords}, Mid: {midLevelRecords}, Code: {codeRecords}");
            Console.Write("Enter 'y' to proceed: ");
            if(Console.ReadLine().Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                dbContext.SaveChanges();
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
                        
                        while(csv.Read())
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
    }
}

