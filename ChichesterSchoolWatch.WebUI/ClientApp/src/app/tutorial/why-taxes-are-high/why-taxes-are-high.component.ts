import { Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
 

@Component({
  selector: 'app-why-taxes-are-high',
  templateUrl: './why-taxes-are-high.component.html',
  styleUrls: ['./why-taxes-are-high.component.css'], 
})
export class WhyTaxesAreHighComponent implements OnInit {
  public steps : string[];
  public currentStep: number = 0;

  constructor(activeRoute: ActivatedRoute) {

    this.steps = [
      'Step1',
      'Step2',
      'homeowner-tax-breakdown-chart'
    ]; 

    //subscribe to route change?
    activeRoute.pathFromRoot.forEach(route => route.params.subscribe(params => {

      //try and locate the step from the route parameter
      let step = params["step"] || "intro";
      let match = this.steps.find(value => value.toLowerCase() === step.toLowerCase());

      //use the match that was found if it is not null
      this.currentStep = (match != null) ? this.steps.indexOf(match) : 0; 
    }));
  }
 
  ngOnInit() { 
  }

  get currentStepName(): string {
    return this.steps[this.currentStep];
  }
 
  get allowPrevious(): boolean {
    return this.currentStep > 0;
  }

  prev() {
    if (this.allowPrevious)
      this.currentStep--;
  }
 
  next() {
    if (this.allowNext)
      this.currentStep++;
  }

  get allowNext(): boolean {
    return this.currentStep <= this.steps.length;
  }
}
