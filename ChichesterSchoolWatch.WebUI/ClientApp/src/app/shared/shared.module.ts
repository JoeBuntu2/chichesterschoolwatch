import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThousandSuffPipe } from './pipes/thousand-suff.pipe';
import { BusyIndicatorComponent } from './components/busy-indicator/busy-indicator.component';
import { LightboxImageDirective } from './components/lightbox-image.directive';
import { LightboxModule } from 'ngx-lightbox';

@NgModule({
  declarations: [
    ThousandSuffPipe, 
    BusyIndicatorComponent, 
    LightboxImageDirective],
  exports: [
    ThousandSuffPipe, 
    BusyIndicatorComponent, 
    LightboxImageDirective,
  ],
  imports: [
    CommonModule,
    LightboxModule
  ]
})
export class SharedModule { }
