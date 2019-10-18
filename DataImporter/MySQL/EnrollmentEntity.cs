using System.ComponentModel.DataAnnotations; 

namespace DataImporter.MySQL
{
    public class EnrollmentEntity
    {
        [Key()]
        public int EnrollmentId { get; set; }
        public int SchoolId { get; set; }
        public int FiscalYearid { get; set; }
        public int GradeLevel { get; set; }
        public int Enrollment { get; set; }
    }
}
