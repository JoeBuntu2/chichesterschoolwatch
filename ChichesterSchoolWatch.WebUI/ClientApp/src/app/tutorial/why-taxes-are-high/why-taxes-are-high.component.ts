import { Component, OnInit} from '@angular/core';
 

@Component({
  selector: 'app-why-taxes-are-high',
  templateUrl: './why-taxes-are-high.component.html',
  styleUrls: ['./why-taxes-are-high.component.css'], 
})
export class WhyTaxesAreHighComponent implements OnInit {
  public steps : string[];
  public currentStep: number; 
 
  ngOnInit() {
    this.steps = [
      'Step1',
      'Step2',
      'homeowner-tax-breakdown-chart'
    ];
    this.currentStep = 0; 
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
