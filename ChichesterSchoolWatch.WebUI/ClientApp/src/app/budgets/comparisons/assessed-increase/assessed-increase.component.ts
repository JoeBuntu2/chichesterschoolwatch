import { Component } from '@angular/core';
import { forkJoin } from 'rxjs';   
import { DistrictApiService } from 'src/app/model/district-api.service';
import { DistrictComparisionApiService } from 'src/app/model/district-comparision-api.service';
import { District } from 'src/app/model/district';
import { DistrictComparison } from 'src/app/model/district-comparison';

@Component({
  selector: 'app-assessed-increase',
  templateUrl: './assessed-increase.component.html',
  styleUrls: ['./assessed-increase.component.css']
})
export class AssessedIncreaseComponent {
  public isBusy: boolean;
  public districts: District[];
  public comparisons: DistrictComparison[]; 
  public condensed: boolean = true;

  constructor( 
    private districtsApi : DistrictApiService,
    private districtComparisonsApi : DistrictComparisionApiService) {
  
    this.isBusy = true;
 
    forkJoin([
      districtComparisonsApi.getComparisons(),
      districtsApi.getDistricts() 
    ]).subscribe(results => {

        this.comparisons = results[0];
        this.districts = results[1];

        this.isBusy = false;
      },
      error => console.error(error)
    );
  }
 

}
