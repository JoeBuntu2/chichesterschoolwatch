import { Component} from '@angular/core'; 
import { forkJoin } from 'rxjs';  
import { District } from 'src/app/model/district';
import { DistrictApiService } from 'src/app/model/district-api.service';
import { DistrictComparisionApiService } from 'src/app/model/district-comparision-api.service';
import { DistrictComparison } from 'src/app/model/district-comparison';
import { DistrictNameSorterService } from 'src/app/model/district-name-sorter.service';

@Component({
  selector: 'app-assessed',
  templateUrl: './assessed.component.html',
  styleUrls: ['./assessed.component.css']
})
export class AssessedComponent  {
  public isBusy: boolean;
  public districts: District[];
  public comparisons: DistrictComparison; 
  public condensed: boolean = true;

  constructor(  
    private districtsApi: DistrictApiService,
    private districtComparisonsApi: DistrictComparisionApiService,
    private districtNameSorter: DistrictNameSorterService
    )
    {
  
    this.isBusy = true;
 
    forkJoin([
      districtComparisonsApi.getComparisons(),
      districtsApi.getDistricts()
    ]).subscribe(results => {

        this.comparisons = results[0];
 
        let districtsResults = results[1];
        this.districts = districtsResults.sort(districtNameSorter.sort);

        this.isBusy = false;
      },
      error => console.error(error)
    );
  }

}
