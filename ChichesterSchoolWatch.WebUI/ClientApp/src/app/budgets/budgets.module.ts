import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BudgetsComponent } from './budgets.component';
import { RevenuesComponent } from './revenues/revenues.component';
import { ExpendituresComponent } from './expenditures/expenditures.component';
import { RouterModule } from '@angular/router';
import { ComparisonsComponent } from './comparisons/comparisons.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

let routing = RouterModule.forChild([
  { path: "expenditures", component: ExpendituresComponent },
  { path: "revenues", component: RevenuesComponent },
  { path: "comparisons/:comp-type", component: ComparisonsComponent },
  { path: "", component: BudgetsComponent }
])
@NgModule({
  imports: [
    CommonModule, routing,
    NgbModule
  ],
  declarations: [BudgetsComponent, RevenuesComponent, ExpendituresComponent, ComparisonsComponent]
})
export class BudgetsModule { }
