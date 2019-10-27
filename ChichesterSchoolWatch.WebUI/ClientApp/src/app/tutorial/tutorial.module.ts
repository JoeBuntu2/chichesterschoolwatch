import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { WhyTaxesAreHighComponent } from './why-taxes-are-high/why-taxes-are-high.component';
import { Step1Component } from './why-taxes-are-high/step1/step1.component';
import { Step2Component } from './why-taxes-are-high/step2/step2.component';
import { StepHostDirective } from './why-taxes-are-high/step-host.directive';
import { ChartsModule } from 'ng2-charts';

let routing = RouterModule.forChild([
  { path: "tax-payers-guide-to-high-chi-taxes", component: WhyTaxesAreHighComponent }
]);

@NgModule({
  declarations: [
    WhyTaxesAreHighComponent, Step1Component, Step2Component, StepHostDirective
  ],
  imports: [
    routing,
    ChartsModule,
    CommonModule
  ],
  exports:[Step1Component, Step2Component],
  entryComponents: [
    Step1Component, Step2Component] 
})
export class TutorialModule { }
