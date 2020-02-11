 
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { PsersNarrativePart1Component } from './psers-narrative-part1/psers-narrative-part1.component';
import { PsersNarrativePart2Component } from './psers-narrative-part2/psers-narrative-part2.component';

let routing = RouterModule.forChild([
  { path: "2020/2/6/psers-narrative-part-1-actual-impact", component: PsersNarrativePart1Component },
  { path: "2020/2/11/psers-narrative-part-2-bartholf-misleads", component: PsersNarrativePart2Component }
]);
@NgModule({
  declarations: [PsersNarrativePart1Component, PsersNarrativePart2Component],
  imports: [
    CommonModule,
    routing,
    SharedModule
  ]
})
export class BlogModule { }
