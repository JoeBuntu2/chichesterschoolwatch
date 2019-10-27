select Districts.Name, 
       Expenditures.TopLevelId,
       Expenditures.MidLevelId,
       Expenditures.CodeId,
       sum(case when FiscalYears.Name = 'FY15-16' THEN Amount else 0 end) as `FY15-16`,
       sum(case when FiscalYears.Name = 'FY16-17' THEN Amount else 0 end) as `FY16-17`,
       sum(case when FiscalYears.Name = 'FY17-18' THEN Amount else 0 end) as `FY17-18`,
       sum(case when FiscalYears.Name = 'FY18-19' THEN Amount else 0 end) as `FY18-19`,
       sum(case when FiscalYears.Name = 'FY19-20' THEN Amount else 0 end) as `FY19-20`

from Expenditures
join Budgets on Budgets.BudgetId = Expenditures.BudgetId
join Districts on Districts.DistrictId = Budgets.DistrictId
join FiscalYears on FiscalYears.FiscalYearId = Budgets.FiscalYearId

group by Districts.DistrictId, TopLevelId, MidLevelId, CodeId with rollup;