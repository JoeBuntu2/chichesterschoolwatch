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
  public comparisons: any;
  public objectKeys = Object.keys;
  public currentDistrict: any;
  public currentMetric: any;
  public currentFiscalYear: any;
  public condensed: boolean = true;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {

    this.isBusy = true;

    forkJoin(
        http.get<any>(baseUrl + 'api/DistrictComparisons'),
        http.get<any[]>(baseUrl + 'api/Districts')
      )
      .subscribe(([comparisons, districts]) => {
          this.comparisons = comparisons;
          this.districts = districts.sort((a, b) => a.name.localeCompare(b.name));

   
            let match = this.comparisons.districtFiscalYearMetrics
              .find(x =>
                x.district.name.toLowerCase().startsWith("PennDelco".toLowerCase()
                ));

            if (match != null) {
              let district = match.district;
              let fyMetrics = match.metricsByFiscalYear[5]; //2019-2020 metrics
              let excessSpendingMetrics = fyMetrics.metrics.ExcessChichesterSpending;
              this.setCurrent(district, 5, excessSpendingMetrics);
            } 
 
          this.isBusy = false;
        },
        error => console.error(error)
      );
  }

  setCurrent(district: any, key: number, metric: any) {
    this.currentDistrict = district;
    this.currentMetric = metric;
    this.currentFiscalYear = this.comparisons.fiscalYears[key];
  }

}
