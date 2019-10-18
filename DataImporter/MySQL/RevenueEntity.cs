using System.ComponentModel.DataAnnotations;

namespace DataImporter.MySQL
{
    public class RevenueEntity
    { 
        public int RevenueId { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
    }
}
