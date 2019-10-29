import { Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
 

@Component({
  selector: 'app-why-taxes-are-high',
  templateUrl: './why-taxes-are-high.component.html',
  styleUrls: ['./why-taxes-are-high.component.css'], 
})
export class WhyTaxesAreHighComponent implements OnInit {
  public steps : any[];
  public currentStep: number = 0;

  constructor(activeRoute: ActivatedRoute) {

    this.steps = [
      { name: 'intro-welcome', display: 'Welcome! Many reasons to care.' },
      { name: 'intro-topics', display: 'Topics we will be covering.' },
      { name: 'excess-cost-per-student', display: "Excessive Cost-Per-Student"},
      { name: 'homeowner-tax-breakdown-chart', display: "tax-break-down"},
      { name: 'stay-tuned', display: 'Stay Tuned for Completion!' }
      //{ name: 'excess-cost-per-student', display: 'Excess Cost Per Student' },
      //{ name: 'excess-spending', display: 'Excess Spending' }
    ]; 

    //subscribe to route change?
    activeRoute.pathFromRoot.forEach(route => route.params.subscribe(params => {

      //try and locate the step from the route parameter
      let stepParameter = params["step"] || "intro";
      let match = this.steps.find(step => step.name.toLowerCase() === stepParameter.toLowerCase());

      //use the match that was found if it is not null
      this.currentStep = (match != null) ? this.steps.indexOf(match) : 0; 
    }));
  }
 
  ngOnInit() { 
  }

  get currentStepName(): string {
    return this.steps[this.currentStep].name;
  }
 
  get allowPrevious(): boolean {
    return this.currentStep > 0;
  }

  get allowNext(): boolean {
    return this.currentStep < (this.steps.length - 1);
  }

  get prevStep(): string {
    if (this.allowPrevious)
      return this.steps[this.currentStep - 1].name;
    return null;
  
  }

  get nextStep(): string {
    if (this.allowNext)
      return  this.steps[1 + this.currentStep].name;
    return null;
  }


}
