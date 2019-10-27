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
      'Step2'
    ];
    this.currentStep = 0; 
  } 
  
}
