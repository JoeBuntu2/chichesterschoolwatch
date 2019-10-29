import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { WhyTaxesAreHighComponent } from './why-taxes-are-high/why-taxes-are-high.component';
import { StepHostDirective } from './why-taxes-are-high/step-host.directive';
import { ChartsModule } from 'ng2-charts';
import { PropertyTaxOverspendingChartComponent } from './why-taxes-are-high/property-tax-overspending-chart/property-tax-overspending-chart.component';
import { HomeownerTaxBreakdownChartComponent } from './why-taxes-are-high/homeowner-tax-breakdown-chart/homeowner-tax-breakdown-chart.component';
import { IntroTopicsComponent } from './why-taxes-are-high/intro/intro-topics.component';
import { IntroWelcomeComponent } from './why-taxes-are-high/intro/intro-welcome.component';
import { ExcessCostPerStudentComponent } from './why-taxes-are-high/excess/excess-cost-per-student.component';
import { StayTunedComponent } from './why-taxes-are-high/stay-tuned/stay-tuned.component';
import { SharedModule } from "../shared/shared.module";
import { ExcessExpendituresComponent } from './why-taxes-are-high/excess/excess-expenditures.component';

let routing = RouterModule.forChild([
  { path: "tax-payers-guide-to-high-chi-taxes", redirectTo: 'tax-payers-guide-to-high-chi-taxes/intro-welcome', pathMatch: 'full'},
  { path: "tax-payers-guide-to-high-chi-taxes/:step", component: WhyTaxesAreHighComponent }
]);

@NgModule({
  declarations: [
    WhyTaxesAreHighComponent,
    StepHostDirective,
    PropertyTaxOverspendingChartComponent,
    HomeownerTaxBreakdownChartComponent,
    IntroTopicsComponent,
    IntroWelcomeComponent,
    ExcessCostPerStudentComponent,
    StayTunedComponent,
    ExcessExpendituresComponent
  ],
  imports: [
    routing,
    ChartsModule,
    CommonModule,
    SharedModule
  ]
})
export class TutorialModule { }
