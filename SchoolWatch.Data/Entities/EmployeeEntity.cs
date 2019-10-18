using System.ComponentModel.DataAnnotations;

namespace SchoolWatch.Data.Entities
{ 
    public class EmployeeEntity
    {
        [Key]   
        public int EmployeeId { get; set; }
        public string Degree { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Step { get; set; } 
        public int? Credit { get; set; } 
        public decimal Salary { get; set; } 
        public  string Location { get; set; } 
        public string Position { get; set; }
    }
}
