import { Component } from '@angular/core';
import { forkJoin } from 'rxjs';  
import { Lightbox } from 'ngx-lightbox';
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
  private album: any[];
  public condensed: boolean = true;

  constructor( 
    private districtsApi : DistrictApiService,
    private districtComparisonsApi : DistrictComparisionApiService, 
    private lightbox: Lightbox) {
  
    this.isBusy = true;

    this.album = [ {
      src: "https://cdn.chichesterschoolwatch.com/bartholf-do-the-math-lowest-in-the-county.png",
      caption: "Bartholf bludgens tax-payer with DCIU tax comparison chart",
      thumb: "https://cdn.chichesterschoolwatch.com/bartholf-do-the-math-lowest-in-the-county.png"

    }];
 
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

  open(index: number): void {
    // open lightbox
    this.lightbox.open(this.album, index);
  }


}
