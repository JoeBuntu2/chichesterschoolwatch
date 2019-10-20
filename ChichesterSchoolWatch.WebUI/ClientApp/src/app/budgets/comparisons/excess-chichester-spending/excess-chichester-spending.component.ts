import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-excess-chichester-spending',
  templateUrl: './excess-chichester-spending.component.html',
  styleUrls: ['./excess-chichester-spending.component.css']
})
export class ExcessChichesterSpendingComponent {
 
  public isBusy: boolean;
  public districts: any[];
  public comparisons: any[];
  public objectKeys = Object.keys;
  public currentDistrict: any;
  public currentMetric: any;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {
  
    this.isBusy = true;
 
    forkJoin([
      http.get<any[]>(baseUrl + 'api/DistrictComparisons'),
      http.get<any[]>(baseUrl + 'api/Districts') 
    ]).subscribe(results => {

        this.comparisons = results[0];
 
        let districtsResults = results[1];
        this.districts = districtsResults.sort((a, b) => a.name.localeCompare(b.name));

        this.isBusy = false;
      },
      error => console.error(error)
    );
  }

  setCurrent(district: any, metric: any) {
    this.currentDistrict = district;
    this.currentMetric = metric;
  }

}
