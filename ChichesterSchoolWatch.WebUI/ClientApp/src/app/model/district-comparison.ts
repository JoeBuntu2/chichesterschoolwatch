import { District } from "./district"

export class DistrictComparison {
    public districtFiscalYearMetrics : DistrictFiscalYearMetrics[]
    public fiscalYears: Record<number, string>
}

 
export class DistrictFiscalYearMetrics {
    public district : District;
    public metricsByFiscalYear : Record<number, DistrictMetrics>;
}

export class DistrictMetrics {
    public Assessed : number;
    public AssessedPerStudent : number;
    public CostPerStudentComparedToChichester : number;
    public Deficit : number;
    public DeficitPerStudent : number;    
}