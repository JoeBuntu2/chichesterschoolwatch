import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';
import { Lightbox } from 'ngx-lightbox';
import { StatsService } from "../../../stats.service";  

@Component({
  selector: 'app-deficit-per-student',
  templateUrl: './deficit-per-student.component.html',
  styleUrls: ['./deficit-per-student.component.css']
})
export class DeficitPerStudentComponent  {
 
  public isBusy: boolean;
  public districts: any[];
  public comparisons: any[]; 
  private album: any[];
  public condensed: boolean = true;
  constructor(
    private http: HttpClient,
    private lightbox: Lightbox,
    public stats: StatsService,
    @Inject('BASE_URL') baseUrl: string) {
 
    this.isBusy = true;

    this.album = [ {
      src: "https://cdn.chichesterschoolwatch.com/bartholf-touts-zero-percent.png",
      caption: "Bartholf forgets Deficit, Touts Zero Percent Increase.",
      thumb: "https://cdn.chichesterschoolwatch.com/bartholf-touts-zero-percent.png"

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
