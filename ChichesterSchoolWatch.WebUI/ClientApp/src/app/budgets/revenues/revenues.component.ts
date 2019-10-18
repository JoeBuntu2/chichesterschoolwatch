import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-revenues',
  templateUrl: './revenues.component.html',
  styleUrls: ['./revenues.component.css']
})
export class RevenuesComponent implements OnInit {

  public allDistrictRevenues: any[];
  public selectedRevenues: any[];
  public districts: any;
  public selectedDistrict: any;
  public selectedZoomOption: any;
  public zoomOptions: any[];
  public codes: any[];
  public isBusy: boolean;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    this.isBusy = true;

    this.zoomOptions = [
      { name: "High" },
      { name: "Mid" } 
    ];
    this.setZoomOption(this.zoomOptions[1]); //mid
 
    forkJoin([
      http.get<any[]>(baseUrl + 'api/Revenues'),
      http.get<any[]>(baseUrl + 'api/Districts'),
      http.get<any[]>(baseUrl + 'api/RevenueCodes')
    ]).subscribe(results => {

        let expenditureResults = results[0];
        this.allDistrictRevenues = expenditureResults;

        this.codes = results[2];

        let districtsResults = results[1];
        this.districts = districtsResults.sort((a, b) => a.name.localeCompare(b.name));

        let chi = this.districts.filter(x => x.name.toLowerCase().startsWith("chichester"))[0];
        this.setDistrict(chi);

        this.isBusy = false;
      },
      error => console.error(error)
    );
  }

  ngOnInit() {
  }

  setZoomOption(zoomOption: any) {

    this.zoomOptions.forEach(option => option.isActive = false);
    zoomOption.isActive = true;

    this.selectedZoomOption = zoomOption;
  }

  setDistrict(district: any) {
    if (!district)
      return;
    if (this.selectedDistrict) {
      this.selectedDistrict.isActive = false;
    }
    this.selectedDistrict = district;
    this.selectedDistrict.isActive = true;
    if (this.allDistrictRevenues) {
      this.selectedRevenues =
        this.allDistrictRevenues.filter(district => district.districtId === this.selectedDistrict.districtId);
    }
  }
}
