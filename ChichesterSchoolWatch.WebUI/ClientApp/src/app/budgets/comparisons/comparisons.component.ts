import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-comparisons',
  templateUrl: './comparisons.component.html',
  styleUrls: ['./comparisons.component.css']
})
export class ComparisonsComponent  {
  public districts: any[];
  public comparisions: any[];
  public isBusy: boolean;
  public objectKeys = Object.keys;
  public options: any[];
  public optionGroups: any[];
  public selectedOption: any;

  constructor(
      private http: HttpClient,
        activeRoute: ActivatedRoute,
        @Inject('BASE_URL') baseUrl: string) {

    this.loadOptions();

    //subscribe to route change?
    activeRoute.pathFromRoot.forEach(route => route.params.subscribe(params => {

      let comparisonType = params["comp-type"] || "TotalCostPerStudent";
      comparisonType = comparisonType.replace(/-/g, "").toLowerCase();

      let match = this.options.find(value => value.name.toLowerCase() === comparisonType);
      if (match == null)
          match = this.options.find(value => value.name.toLowerCase() === "TotalCostPerStudent".toLowerCase());

      this.selectedOption = match;
 
    }));
 
    this.isBusy = true;
 
    forkJoin([
      http.get<any[]>(baseUrl + 'api/DistrictComparisons'),
      http.get<any[]>(baseUrl + 'api/Districts') 
    ]).subscribe(results => {

        this.comparisions = results[0];
 
        let districtsResults = results[1];
        this.districts = districtsResults.sort((a, b) => a.name.localeCompare(b.name));

        this.isBusy = false;
      },
      error => console.error(error)
    );
  }

  loadOptions() {

    this.optionGroups = [
      {
        group: "Over Spending",
        groupOptions: [
          { name: "Deficit", format: "number"},
          { name: "DeficitPerStudent", format: "number"},
          { name: "ExcessChichesterSpending", format: "number" },
          { name: "CostPerStudentComparedToChichester", format: "number" }
        ]
      },
      {
          group: "Revenue",
          groupOptions: [
            { name: "TotalRevenue", format: "number" },
            { name: "TotalRevenuePerStudent", format: "number" },
            { name: "RevenueIncrease", format: "number" },
            { name: "RevenueIncreasePerStudent", format: "number" }
        ]
      },
      {
        group: "Expenditures",
        groupOptions: [
          { name: "TotalCost", format: "number" },
          { name: "TotalCostPerStudent", format: "number" },
          { name: "TotalCostIncrease", format: "number" },
          { name: "TotalCostIncreasePerStudent", format: "number" }

        ]
      },
      {
        group: "Assessments",
        groupOptions: [
          { name: "Assessed", format: "number"},
          { name: "AssessedPerStudent", format: "number"},
          { name: "AssessedIncrease", format: "number"},
          { name: "AssessedNewRevenue", format: "number"}
        ]
      },
      {
        group: "Board Talking Points",
        groupOptions: [
          { name: "TaxRateIncrease", format: "percent"},
          { name: "SpecialEducation1200PercentageCost", format: "percent"}
        ]
      },
    ];

    this.options = []; 
    this.optionGroups.forEach(
      group => group.groupOptions.forEach(
          option => this.options.push(option)
      )
    );

    //turn 
    this.options.forEach(option => {
      option.canonicalUrl = option.name
        .replace(/([A-Z])/g, (match) => `-${match.toLowerCase()}`).substr(1);
    });
  }


  setOption(option: any) {

    this.options.forEach(o => o.isActive = false);
    option.isActive = true;

    this.selectedOption = option;
  }
}
