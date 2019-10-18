using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.TypeConversion;
using DataImporter.FileRecords;
using DataImporter.MySQL;
using Microsoft.Extensions.DependencyInjection;

namespace DataImporter.ImportTasks
{
    public class ImportEnrollmentTask 
    {
        private Dictionary<string, int> SchoolLookup;
        private Dictionary<int, FiscalYearEntity> FiscalYearLookup;

        public void Run(ServiceProvider serviceProvider, int districtId)
        { 
            //configure console logging
            var dbContext = serviceProvider.GetService<SchoolDbContext>();
            

            SchoolLookup = dbContext.Set<SchoolEntity>().Where(x => x.DistrictId == districtId)
                .ToDictionary(x => x.Name.Replace(" ", ""), x => x.SchoolId);
          

            FiscalYearLookup = dbContext.Set<FiscalYearEntity>()
                .ToDictionary(x => x.Start, x => x);

            var records = ReadEnrollmentFileRecords();
            var entities = Transform(records);

            dbContext.Set<EnrollmentEntity>().AddRange(entities);
            dbContext.SaveChanges();
        }

        private List<EnrollmentEntity> Transform(List<EnrollmentFileRecord> fileRecords)
        {
            var entities = new List<EnrollmentEntity>();

            foreach (var fileRecord in fileRecords)
            {
                entities.Add(Transform(fileRecord));
            }

            return entities;
        }

        

        private EnrollmentEntity Transform(EnrollmentFileRecord fileRecord)
        {
            var entity = new EnrollmentEntity(); 
            entity.SchoolId = SchoolLookup[fileRecord.Building.Replace(" ", "")];

            if (fileRecord.Grade.Trim() == "K")
                entity.GradeLevel = 0;
            else
                entity.GradeLevel = int.Parse(fileRecord.Grade);

            entity.FiscalYearid = FiscalYearLookup[int.Parse(fileRecord.Year)].FiscalYearId;
            entity.Enrollment = fileRecord.Enrollment;
            return entity;
        }

        private  List<FileRecords.EnrollmentFileRecord> ReadEnrollmentFileRecords()
        { 
            using (var fs = File.OpenRead("./Imports/EnrollmentAltered.csv"))
            {
                using (var textReader = new StreamReader(fs))
                {
                    using (var csv = new CsvReader(textReader))
                    {
                        //csv.Configuration.TypeConverterOptionsCache.AddOptions<decimal>(
                        //    new TypeConverterOptions { NumberStyle = NumberStyles.Currency }
                        //);
                        csv.Read();
                        csv.ReadHeader();
                        return csv.GetRecords<EnrollmentFileRecord>().ToList();
                    }
                }
            }

        }
    }
}
