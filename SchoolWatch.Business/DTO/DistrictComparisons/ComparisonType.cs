namespace SchoolWatch.Business.DTO.DistrictComparisons
{
    public enum ComparisonType
    {  
        TotalRevenue,
        TotalRevenuePerStudent,
        RevenueIncrease,
        RevenueIncreasePerStudent,
 
        TotalCost,
        TotalCostPerStudent,
        TotalCostIncrease,
        TotalCostIncreasePerStudent,

        CostPerStudentComparedToChichester, //diff in cost-per-student
        ExcessChichesterSpending, //applying diff in cost-per-student x enrollment

        SpecialEducation1200PercentageCost,

        Deficit,
        DeficitPerStudent,
 

        TaxRateIncrease,

        Assessed,
        AssessedPerStudent,
        AssessedIncrease,
        AssessedNewRevenue,
        AssessedNewRevenuePerStudent,

        Enrollment

    }
}
