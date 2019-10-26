import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';
import { Lightbox } from 'ngx-lightbox';

@Component({
  selector: 'app-special-education1200-percentage-cost',
  templateUrl: './special-education1200-percentage-cost.component.html',
  styleUrls: ['./special-education1200-percentage-cost.component.css']
})
export class SpecialEducation1200PercentageCostComponent  {
 
  public isBusy: boolean;
  public districts: any[];
  public comparisons: any; 
  public currentDistrict: any;
  public currentMetric: any;
  public currentFiscalYear: any;
  private album: any[];
  public condensed: boolean = true;

  constructor(
    private http: HttpClient,
    private lightbox: Lightbox,
    @Inject('BASE_URL') baseUrl: string) {

    this.isBusy = true;

    this.album = [ {
      src: "../assets/images/bartholf-misleads-on-special-education-budget-impact.png",
      caption: "Bartholf suggests chichester spends more on special education than other districts.",
      thumb: "../assets/images/bartholf-misleads-on-special-education-budget-impact.png",

    }];

    forkJoin(
        http.get<any>(baseUrl + 'api/DistrictComparisons'),
        http.get<any[]>(baseUrl + 'api/Districts')
      )
      .subscribe(([comparisons, districts]) => {
          this.comparisons = comparisons;
          this.districts = districts.sort((a, b) => a.name.localeCompare(b.name));
 
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

    
  open(index: number): void {
    // open lightbox
    this.lightbox.open(this.album, index);
  }
}
