import { Component, OnInit, Inject } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-psers-contributions-breakdown',
  templateUrl: './psers-contributions-breakdown.component.html',
  styleUrls: ['./psers-contributions-breakdown.component.css']
})
export class PsersContributionsBreakdownComponent  {

  public barChartOptions: ChartOptions = {
     responsive: true,
     maintainAspectRatio: false,
     title: {
       display: true,
       text: 'Delco Districts Cost-Per-Student',
       fontSize: 16
     },
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{
      scaleLabel: {
        display: true,
        labelString: 'Cost-Per-Student'
      },
      display: true,
      ticks: {
        beginAtZero: false,
        min: 15000,
        max: 28000
      }
    }] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };


  public barChartLabels: Label[] = [];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [pluginDataLabels];

  public barChartData: ChartDataSets[] = [];
  public isBusy: boolean;
  public districts: any[];
  public comparisons: any;
  public condensed: boolean = true;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {

    this.isBusy = true;

    forkJoin(
        http.get<any>(baseUrl + 'api/DistrictComparisons') 
      )
      .subscribe(([comparisons]) => {
          this.comparisons = comparisons;

          //get the last two fiscal years
          const keys = Object.keys(this.comparisons.fiscalYears).sort((a,b) =>  -1 * a.localeCompare(b)).slice(0,1);

          //load fiscal years
          this.barChartLabels = [];
          keys.forEach(key => {
            var fy = this.comparisons.fiscalYears[key];
            this.barChartLabels.push(<Label> fy);
          });

          //sort districts by name
          const sortedDistrictMetrics =
            comparisons.districtFiscalYearMetrics.sort((a, b) => a.district.name.localeCompare(b.district.name));

          //foreach district set of data
          sortedDistrictMetrics.forEach(districtComparisonData => {
 
            let data = [];  

            //foreach fy metric set of data
            keys.forEach(key => {
              let fyMetrics = districtComparisonData.metricsByFiscalYear[key];

              let metric = Math.round(fyMetrics.metrics['TotalCostPerStudent']);
              data.push(metric);

            });
 
            let districtData: ChartDataSets = { data: data, label: districtComparisonData.district.name };
            this.barChartData.push(districtData);
          });


          this.isBusy = false;
        },
        error => console.error(error)
      );
  }
}
