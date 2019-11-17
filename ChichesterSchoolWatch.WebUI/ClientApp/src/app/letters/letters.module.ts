import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { BudgetsModule } from '../budgets/budgets.module'
import { LetterChichesterPSERSNetContributionsComponent } from './letter-chichester-psers-net-contributions/letter-chichester-psers-net-contributions.component';

let routing = RouterModule.forChild([
  { path: "board/psers-net-contributions-accuracy", component: LetterChichesterPSERSNetContributionsComponent }
]);
@NgModule({
  declarations: [LetterChichesterPSERSNetContributionsComponent],
  imports: [
    CommonModule,
    routing,
    BudgetsModule
  ]
})
export class LettersModule { }
