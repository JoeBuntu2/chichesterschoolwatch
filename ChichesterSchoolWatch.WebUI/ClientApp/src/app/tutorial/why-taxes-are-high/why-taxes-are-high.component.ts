import { Component, OnInit, ViewChild, ComponentFactoryResolver, Host, Injector, NgModuleRef } from '@angular/core';
import {Step1Component} from './step1/step1.component';
import {Step2Component} from './step2/step2.component';
import {StepHostDirective} from './step-host.directive';

@Component({
  selector: 'app-why-taxes-are-high',
  templateUrl: './why-taxes-are-high.component.html',
  styleUrls: ['./why-taxes-are-high.component.css'],
  entryComponents: [Step1Component, Step2Component]
})
export class WhyTaxesAreHighComponent implements OnInit {
  public steps : any[];
  public currentStep: number;
  @ViewChild(StepHostDirective) stepHost: StepHostDirective;


  constructor(
      private step1 : Step1Component,
      private step2: Step2Component,
      private moduleRef: NgModuleRef<any>
  ) { }

  ngOnInit() {
    this.steps = [
      this.step1, this.step2
    ];
    this.currentStep = 0;
    this.loadComponent(); 
  } 
 
  loadComponent() { 
    const step = this.steps[this.currentStep];

    const componentFactoryResolver = this.moduleRef.componentFactoryResolver;
    const componentFactory = componentFactoryResolver.resolveComponentFactory(step);

    const viewContainerRef = this.stepHost.viewContainerRef;
    viewContainerRef.clear();

    const componentRef = viewContainerRef.createComponent(componentFactory); 
  }

}
