import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-salaries',
  templateUrl: './salaries.component.html',
  styleUrls: ['./salaries.component.css']
})
export class SalariesComponent {
  public salaries: Salary[];
  public isBusy: boolean;
  
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.isBusy = true;

    http.get<Salary[]>(baseUrl + 'api/Salaries').subscribe(result => {
      this.salaries = result;
      this.isBusy = false;
    }, error => console.error(error));
  }

}

interface Salary {
  position: string,
  credit: number,
  degree: string,
  step: number,
  salary: number,
  location: string
}
