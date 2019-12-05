import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartsModule } from 'ng2-charts';
import { BudgetsComponent } from './budgets.component';
import { RevenuesComponent } from './revenues/revenues.component';
import { ExpendituresComponent } from './expenditures/expenditures.component';
import { RouterModule } from '@angular/router';
import { ComparisonsComponent } from './comparisons/comparisons.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CostPerStudentComponent } from './comparisons/cost-per-student/cost-per-student.component';
import { DeficitComponent } from './comparisons/deficit/deficit.component';
import { DeficitPerStudentComponent } from './comparisons/deficit-per-student/deficit-per-student.component';
import { ExcessChichesterSpendingComponent } from './comparisons/excess-chichester-spending/excess-chichester-spending.component';
import { TaxRateIncreaseComponent } from './comparisons/tax-rate-increase/tax-rate-increase.component';
import { SharedModule } from '../shared/shared.module';
import { LightboxModule } from 'ngx-lightbox';
import { OrderModule } from 'ngx-order-pipe';
import { SpecialEducation1200PercentageCostComponent } from './comparisons/special-education1200-percentage-cost/special-education1200-percentage-cost.component';
import { ComparisonsGridComponent } from './comparisons/comparisons-grid/comparisons-grid.component';
import { ComparisonOptionsComponent } from './comparisons/comparison-options/comparison-options.component';
import { AssessedIncreaseComponent } from './comparisons/assessed-increase/assessed-increase.component';
import { AssessedNewRevenueComponent } from './comparisons/assessed-new-revenue/assessed-new-revenue.component';
import { AssessedNewRevenuePerStudentComponent } from './comparisons/assessed-new-revenue-per-student/assessed-new-revenue-per-student.component';
import { AssessedComponent } from './comparisons/assessed/assessed.component';
import { PsersNetContributionIncreaseComponent } from './comparisons/psers-net-contribution-increase/psers-net-contribution-increase.component';
import {MatMenuModule} from '@angular/material/menu';
import { PsersContributionsBreakdownComponent } from './charts/psers-contributions-breakdown/psers-contributions-breakdown.component';
import { ModelModule } from '../model/model.module';


let routing = RouterModule.forChild([
  { path: "expenditures", component: ExpendituresComponent },
  { path: "revenues", component: RevenuesComponent },
  { path: "comparisons/:comp-type", component: ComparisonsComponent },
  { path: "", component: BudgetsComponent }
]);
@NgModule({
  imports: [
    CommonModule,
    routing,
    ChartsModule,
    NgbModule,
    ModelModule,
    SharedModule,
    LightboxModule,
    MatMenuModule,
    OrderModule
  ],
  declarations: [
    BudgetsComponent,
    RevenuesComponent,
    ExpendituresComponent,
    ComparisonsComponent,
    CostPerStudentComponent,
    DeficitComponent,
    DeficitPerStudentComponent,
    ExcessChichesterSpendingComponent,
    TaxRateIncreaseComponent,
    SpecialEducation1200PercentageCostComponent,
    ComparisonsGridComponent,
    ComparisonOptionsComponent,
    AssessedIncreaseComponent,
    AssessedNewRevenueComponent,
    AssessedNewRevenuePerStudentComponent,
    AssessedComponent,
    PsersNetContributionIncreaseComponent,
    PsersContributionsBreakdownComponent],
  exports: [
    PsersContributionsBreakdownComponent
    ]
})
export class BudgetsModule { }
