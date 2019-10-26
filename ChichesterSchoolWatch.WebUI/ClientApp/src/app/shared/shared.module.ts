import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThousandSuffPipe } from './pipes/thousand-suff.pipe';
import { BusyIndicatorComponent } from './components/busy-indicator/busy-indicator.component';

@NgModule({
  declarations: [ThousandSuffPipe, BusyIndicatorComponent],
  exports: [
    ThousandSuffPipe, BusyIndicatorComponent
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
