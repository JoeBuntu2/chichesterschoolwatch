namespace SchoolWatch.Data.Entities
{
    public class TotalEnrollmentEntity
    {
        public int TotalDistrictEnrollmentId { get; set; }
        public int DistrictId { get; set; }
        public int FiscalYearId { get; set; }
        public int Enrollment { get; set; }
    }
}
