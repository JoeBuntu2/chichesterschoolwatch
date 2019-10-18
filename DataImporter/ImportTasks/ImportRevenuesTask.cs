using System;
using CsvHelper; 
using DataImporter.FileRecords;
using DataImporter.MySQL;
using Microsoft.Extensions.DependencyInjection; 
using System.Collections.Generic; 
using System.IO;
using System.Linq;  

namespace DataImporter.ImportTasks
{
    public class ImportRevenuesTask : ITask
    {
        private Dictionary<int, RevenueEntity> RevenuesLookup; 

        public void Run(IServiceProvider serviceProvider)
        {
            //configure console logging
            var dbContext = serviceProvider.GetService<SchoolDbContext>();


            RevenuesLookup = dbContext.Set<RevenueEntity>().ToDictionary(x => x.RevenueId, x => x);
 
            var district = ChooseDistrict(dbContext);
            var records = ReadRevenueRecords(district);
            foreach(var record in records)
            {
                if (!RevenuesLookup.ContainsKey(record.RevenueId))
                {
                    dbContext.Set<RevenueEntity>().Add(new RevenueEntity
                    {
                        RevenueId = record.RevenueId,
                        Level = record.Level[0].ToString(),
                        Description = record.Description
                    });
                }
            } 
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
 
        private List<RevenueFileRecord> ReadRevenueRecords(DistrictEntity district)
        {
            using (var fs = File.OpenRead($"./Imports/{district.Name}_Revenues.csv")) 
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

