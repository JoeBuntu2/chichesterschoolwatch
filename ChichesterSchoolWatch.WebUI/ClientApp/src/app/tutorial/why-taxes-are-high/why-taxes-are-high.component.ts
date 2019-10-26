import { Component, OnInit, ViewChild, ComponentFactoryResolver } from '@angular/core';
import {Step1Component} from './step1/step1.component';
import {Step2Component} from './step2/step2.component';
import {StepHostDirective} from './step-host.directive';

@Component({
  selector: 'app-why-taxes-are-high',
  templateUrl: './why-taxes-are-high.component.html',
  styleUrls: ['./why-taxes-are-high.component.css']
})
export class WhyTaxesAreHighComponent implements OnInit {
  public steps : any[];
  public currentStep: number;
  @ViewChild(StepHostDirective) stepHost: StepHostDirective;


  constructor(
    private step1 : Step1Component,
    private step2: Step2Component,
    private componentFactoryResolver: ComponentFactoryResolver
  ) { }

  ngOnInit() {
    this.currentStep = 0;
    this.loadComponent(); 
  }
 

  loadComponent() { 
    const adItem = this.steps[this.currentStep];

    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(adItem.component);

    const viewContainerRef = this.stepHost.viewContainerRef;
    viewContainerRef.clear();

    const componentRef = viewContainerRef.createComponent(componentFactory); 
  }

}
