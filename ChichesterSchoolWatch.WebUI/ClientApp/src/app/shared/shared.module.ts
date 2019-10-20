import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ThousandSuffPipe } from './pipes/thousand-suff.pipe';

@NgModule({
  declarations: [ThousandSuffPipe],
  exports: [
    ThousandSuffPipe
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule { }
