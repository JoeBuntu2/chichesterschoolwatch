import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';  
import { Lightbox } from 'ngx-lightbox';

@Component({
  selector: 'app-assessed-increase',
  templateUrl: './assessed-increase.component.html',
  styleUrls: ['./assessed-increase.component.css']
})
export class AssessedIncreaseComponent {
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
      src: "../assets/images/bartholf-do-the-math-lowest-in-the-county.png",
      caption: "Bartholf bludgens tax-payer with DCIU tax comparison chart",
      thumb: "../assets/images/bartholf-do-the-math-lowest-in-the-county.png"

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
