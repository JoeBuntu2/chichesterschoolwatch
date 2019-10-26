import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SalariesComponent } from './salaries/salaries.component';
import { EmployeesComponent } from './employees.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';

let routing = RouterModule.forChild([
  { path: "salaries", component: SalariesComponent },
  { path: "", component: EmployeesComponent }
])
@NgModule({
  imports: [CommonModule,   routing, SharedModule], 
  declarations: [SalariesComponent, EmployeesComponent]
}) 
export class EmployeesModule { }


