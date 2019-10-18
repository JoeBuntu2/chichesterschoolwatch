using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataImporter
{
    
    public class SalaryRecord
    {
        [Key]
        [CsvHelper.Configuration.Attributes.Ignore]
        public int EmployeeId { get; set; }

        [Name("FirstName")]
        public string FirstName { get; set; }
        [Name("LastName")]
        public string LastName { get; set; }
        [Name("Degree")]
        public string Degree { get; set; }
        [Name("Step")]
        public int? Step { get; set; }
        [Name("Credit")]
        public int? Credit { get; set; }
        [Name("Salary")]
        public decimal Salary { get; set; }
        [Name("Location")]
        public  string Location { get; set; }
        [Name("Position")]
        public string Position { get; set; }
    }
}
