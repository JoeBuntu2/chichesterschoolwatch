using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.TypeConversion;
using DataImporter.MySQL;
using Microsoft.Extensions.DependencyInjection;

namespace DataImporter.ImportTasks
{
    public class ImportSalaryTask : ITask
    {
        public void Run(IServiceProvider serviceProvider)
        { 
            //configure console logging
            var dbContext = serviceProvider
                .GetService<SchoolDbContext>();
            var records = ReadSalaryRecordsFromCsv();

            dbContext.Set<SalaryRecord>().AddRange(records);
            dbContext.SaveChanges();
        }

        private static List<SalaryRecord> ReadSalaryRecordsFromCsv()
        { 


            using (var fs = File.OpenRead("Imports/Salaries.csv"))
            {
                using (var textReader = new StreamReader(fs))
                {
                    using (var csv = new CsvReader(textReader))
                    {
                        csv.Configuration.TypeConverterOptionsCache.AddOptions<decimal>(
                            new TypeConverterOptions { NumberStyle = NumberStyles.Currency }
                        );
                        csv.Read();
                        csv.ReadHeader();
                        return csv.GetRecords<SalaryRecord>().ToList();
                    }
                }
            }

        }
    }
}
