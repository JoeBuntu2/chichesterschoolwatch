using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using DataImporter.FileRecords;
using DataImporter.MySQL;
using Microsoft.Extensions.DependencyInjection;

namespace DataImporter.ImportTasks
{
    public class ImportAllDistrictEnrollmentsTask : ITask
    {
        private FiscalYearEntity[] FiscalYears;
        private DistrictEntity[] Districts;

        public void Run(IServiceProvider serviceProvider)
        {
            //configure console logging
            var dbContext = serviceProvider.GetService<SchoolDbContext>();

            FiscalYears = dbContext.Set<FiscalYearEntity>().ToArray();
            Districts = dbContext.Set<DistrictEntity>().ToArray();

            var fileRecords = ReadRecordsFromFile();
            var entities = ProjectToEntities(fileRecords);

            dbContext.Set<TotalEnrollmentEntity>().AddRange(entities);

            dbContext.SaveChanges();

        }

        private List<TotalEnrollmentEntity> ProjectToEntities(IEnumerable<TotalDistrictEnrollmentFileRecord> fileRecords)
        {
            var entities = new List<TotalEnrollmentEntity>();

            foreach (var fiscalYear in FiscalYears)
            {
                foreach (var fileRecord in fileRecords)
                {
                    var entity = ConvertToTotalEnrollmentEntity(fileRecord, fiscalYear);
                    if(entity != null)
                        entities.Add(entity);
                }
            }
 
            return entities;
        }

        private TotalEnrollmentEntity ConvertToTotalEnrollmentEntity(TotalDistrictEnrollmentFileRecord fileRecord, FiscalYearEntity fiscalYear)
        {
            var totalEnrollmentEntity = new TotalEnrollmentEntity();

            var district = Districts.SingleOrDefault(x => x.Name.Equals(fileRecord.District, StringComparison.InvariantCultureIgnoreCase));
                
            //skip record if we can't find district
            if (district == null)
            {
                Console.WriteLine($"Skipping Record... Unable to find district '{fileRecord.District}'");
                return null;
            }
 
            //get property value using reflection
            var propertyName = fiscalYear.Name.Replace("-", "");
            var amountText = (string)  fileRecord.GetType().GetProperty(propertyName).GetMethod.Invoke(fileRecord, null);

            if (string.IsNullOrWhiteSpace(amountText))
                return null;

            var amount = int.Parse(amountText, System.Globalization.NumberStyles.AllowThousands);

            totalEnrollmentEntity.DistrictId = district.DistrictId;
            totalEnrollmentEntity.Enrollment = amount;
            totalEnrollmentEntity.FiscalYearId = fiscalYear.FiscalYearId;

            return totalEnrollmentEntity;
        }
 
        private List<TotalDistrictEnrollmentFileRecord> ReadRecordsFromFile()
        {
            var totalsSkipped = 0;
            var blanksSkipped = 0;
            var records = new List<TotalDistrictEnrollmentFileRecord>();
            using (var fs = File.OpenRead($"./Imports/All_Enrollments.csv"))
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
                            var record = csv.GetRecord<TotalDistrictEnrollmentFileRecord>();
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
