import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BudgetsModule } from '../budgets/budgets.module'
import { LetterChichesterPSERSNetContributionsComponent } from './letter-chichester-psers-net-contributions/letter-chichester-psers-net-contributions.component';
import { PsersNarrativeComponent } from './psers-narrative/psers-narrative.component';

let routing = RouterModule.forChild([
  { path: "board/bob", component: PsersNarrativeComponent },
  { path: "board/ted", component: LetterChichesterPSERSNetContributionsComponent },
 
  { path: "board/psers-net-contributions-accuracy", component: PsersNarrativeComponent }
]);
@NgModule({
  declarations: [LetterChichesterPSERSNetContributionsComponent, PsersNarrativeComponent],
  imports: [
    CommonModule,
    routing,
    BudgetsModule
  ]
})
export class LettersModule { }
