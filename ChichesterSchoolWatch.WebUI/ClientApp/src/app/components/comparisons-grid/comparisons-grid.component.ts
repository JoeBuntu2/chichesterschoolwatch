import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-comparisons-grid',
  templateUrl: './comparisons-grid.component.html',
  styleUrls: ['./comparisons-grid.component.css']
})
export class ComparisonsGridComponent implements OnInit {

  @Input() metric: string;
  @Input() comparisons: any;
  @Input() isPercent: boolean;

  public fiscalYears: any[]; 


  constructor() {

    this.fiscalYears = [];
  }

  ngOnInit() {
    var keys = Object.keys(this.comparisons.fiscalYears);
    keys.sort((a, b) => a.localeCompare(b) * -1);

    keys.forEach(key => {
      this.fiscalYears.push(
        {
          key: key,
          year: this.comparisons.fiscalYears[key]

        });
    }); 
  }

}
