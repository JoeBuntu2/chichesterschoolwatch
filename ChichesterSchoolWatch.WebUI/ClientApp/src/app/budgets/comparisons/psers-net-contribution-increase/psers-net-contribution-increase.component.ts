import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';  
import { Lightbox } from 'ngx-lightbox';

@Component({
  selector: 'app-psers-net-contribution-increase',
  templateUrl: './psers-net-contribution-increase.component.html',
  styleUrls: ['./psers-net-contribution-increase.component.css']
})
export class PsersNetContributionIncreaseComponent {
  public isBusy: boolean;
  public districts: any[];
  public comparisons: any[];
  private album: any[];
  public condensed: boolean = true;

  constructor( 
    private http: HttpClient, 
    private lightbox: Lightbox,
    @Inject('BASE_URL') baseUrl: string) {
  
    this.isBusy = true;

    this.album = [ {
      src: "https://cdn.chichesterschoolwatch.com/bartholf-exaggerates-psers.png",
      caption: "Bartholf suggests PSERS is straining the district",
      thumb: "https://cdn.chichesterschoolwatch.com/bartholf-exaggerates-psers.png"

    }];
 
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

  open(index: number): void {
    // open lightbox
    this.lightbox.open(this.album, index);
  }


}
