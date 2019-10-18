import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-expenditures',
  templateUrl: './expenditures.component.html',
  styleUrls: ['./expenditures.component.css']
})
export class ExpendituresComponent implements OnInit {

  public allDistrictExpenditures: any[];
  public selectedExpenditures: any[];
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
      { name: "Mid" },
      { name: "Low" }
    ];
    this.setZoomOption(this.zoomOptions[1]); //mid

    forkJoin([
      http.get<any[]>(baseUrl + 'api/Expenditures'),
      http.get<any[]>(baseUrl + 'api/Districts'),
      http.get<any[]>(baseUrl + 'api/ExpenditureCodes')
    ]).subscribe(results => {

        let expenditureResults = results[0];
        this.allDistrictExpenditures = expenditureResults;

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
    if (this.allDistrictExpenditures) {
      this.selectedExpenditures =
        this.allDistrictExpenditures.filter(district => district.districtId === this.selectedDistrict.districtId);
    }
  }
}
 
