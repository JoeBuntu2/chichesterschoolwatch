import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BudgetsComponent } from './budgets.component';
import { RevenuesComponent } from './revenues/revenues.component';
import { ExpendituresComponent } from './expenditures/expenditures.component';
import { RouterModule } from '@angular/router';
import { ComparisonsComponent } from './comparisons/comparisons.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { CostPerStudentComponent } from './comparisons/cost-per-student/cost-per-student.component';
import { DeficitComponent } from './comparisons/deficit/deficit.component';
import { DeficitPerStudentComponent } from './comparisons/deficit-per-student/deficit-per-student.component';
import { ExcessChichesterSpendingComponent } from './comparisons/excess-chichester-spending/excess-chichester-spending.component';
import { TaxRateIncreaseComponent } from './comparisons/tax-rate-increase/tax-rate-increase.component';
import { SharedModule   } from '../shared/shared.module';
import { LightboxModule } from 'ngx-lightbox';
import { OrderModule } from 'ngx-order-pipe';


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
    NgbModule,
    SharedModule,
    LightboxModule,
    OrderModule
  ],
  declarations: [BudgetsComponent, RevenuesComponent, ExpendituresComponent, ComparisonsComponent, CostPerStudentComponent, DeficitComponent, DeficitPerStudentComponent, ExcessChichesterSpendingComponent, TaxRateIncreaseComponent]
})
export class BudgetsModule { }
