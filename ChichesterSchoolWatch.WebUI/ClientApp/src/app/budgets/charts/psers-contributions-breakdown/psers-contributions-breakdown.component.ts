import { Component, OnInit, Input, Inject } from '@angular/core';
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
export class PsersContributionsBreakdownComponent implements OnInit {


  @Input() district: string;

  public barChartOptions: ChartOptions = {
     responsive: true,
     maintainAspectRatio: false,
     title: {
       display: true,
       text: 'Chichester PSERS Contributions',
       fontSize: 16
     },
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{
      scaleLabel: {
        display: true,
        labelString: 'Contribution (Millions)'
      },
      display: true,
      ticks: {
        beginAtZero: false,
        min: 2,
        max: 8
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
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit(): void {
    this.isBusy = true;

    forkJoin(
        this.http.get<any>(this.baseUrl + 'api/DistrictComparisons') 
      )
      .subscribe(([comparisons]) => {
          this.comparisons = comparisons;

          //get the last two fiscal years
          const keys = Object.keys(this.comparisons.fiscalYears);

          //load fiscal years
          this.barChartLabels = [];
          keys.forEach(key => {
            var fy = this.comparisons.fiscalYears[key];
            this.barChartLabels.push(<Label> fy);
          });

          //sort districts by name
          const sortedDistrictMetrics = comparisons.districtFiscalYearMetrics.sort((a, b) => a.district.name.localeCompare(b.district.name));

          //foreach district set of data
          sortedDistrictMetrics.forEach(districtComparisonData => {

            //filter by configured district
            if (districtComparisonData.district.name !== this.district)
              return;

            this.addMetric(districtComparisonData, keys, 'stateContribution', 'State Contribution');
            this.addMetric(districtComparisonData, keys, 'districtNetContribution', 'District Contribution');
          });


          this.isBusy = false;
        },
        error => console.error(error)
      );


  }


  addMetric(districtComparisonData: any, fiscalYearKeys : string[], metricName: string, label: string) {

  let data = [];  

    //foreach fy metric set of data
    fiscalYearKeys.forEach(key => {
      let fyMetrics = districtComparisonData.metricsByFiscalYear[key];

      //get the container for all psers data
      var psersAll = fyMetrics.metrics['PsersAll'];

      //add the psers metric of interest to this data set
      let metric = Math.round(psersAll[metricName] / 10000);
      data.push(metric / 100);

    });
 
    let districtData: ChartDataSets = { data: data, label: label };
    this.barChartData.push(districtData);
  }
}
