select Districts.Name, 
       Revenues.Level,
       Revenues.RevenueId,
       Revenues.Description,
       sum(case when FiscalYears.Name = 'FY15-16' THEN Amount else 0 end) as `FY15-16`,
       sum(case when FiscalYears.Name = 'FY16-17' THEN Amount else 0 end) as `FY16-17`,
       sum(case when FiscalYears.Name = 'FY17-18' THEN Amount else 0 end) as `FY17-18`,
       sum(case when FiscalYears.Name = 'FY18-19' THEN Amount else 0 end) as `FY18-19`,
       sum(case when FiscalYears.Name = 'FY19-20' THEN Amount else 0 end) as `FY19-20`

from BudgetRevenues
join Budgets on Budgets.BudgetId = BudgetRevenues.BudgetId
join Districts on Districts.DistrictId = Budgets.DistrictId
join FiscalYears on FiscalYears.FiscalYearId = Budgets.FiscalYearId
join Revenues on Revenues.RevenueId = BudgetRevenues.RevenueId
group by RevenueId
ORDER BY RevenueId;