 
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PsersNarrativePart1Component } from './psers-narrative-part1/psers-narrative-part1.component';

let routing = RouterModule.forChild([
  { path: "2020/2/4/psers-narrative-part-1-actual-impact", component: PsersNarrativePart1Component }
]);
@NgModule({
  declarations: [PsersNarrativePart1Component],
  imports: [
    CommonModule,
    routing
  ]
})
export class BlogModule { }
