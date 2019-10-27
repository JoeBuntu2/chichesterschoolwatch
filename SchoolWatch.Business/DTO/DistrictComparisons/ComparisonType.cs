namespace SchoolWatch.Business.DTO.DistrictComparisons
{
    public enum ComparisonType
    {  
        TotalRevenue,
        TotalRevenuePerStudent,
        RevenueIncrease,
        RevenueIncreasePerStudent,
        StateRevenue,
        StateRevenuePerStudent,
        StateRevenuePercent,

        TotalCost,
        TotalCostPerStudent,
        TotalCostIncrease,
        TotalCostIncreasePerStudent,

        CostPerStudentComparedToChichester, //diff in cost-per-student
        ExcessChichesterSpending, //applying diff in cost-per-student x enrollment

        SpecialEducation1200PercentageCost,

        Deficit,
        DeficitPerStudent,

        
        PsersAll,
        PsersNetContribution,
        PsersNetContributionIncrease,
        PsersStateContribution,
 

        TaxRateIncrease,

        Assessed,
        AssessedPerStudent,
        AssessedIncrease,
        AssessedNewRevenue,
        AssessedNewRevenuePerStudent,

        Enrollment,

    }
}
