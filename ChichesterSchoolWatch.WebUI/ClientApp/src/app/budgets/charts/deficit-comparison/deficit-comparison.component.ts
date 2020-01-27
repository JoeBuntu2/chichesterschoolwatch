import { Component, OnInit, Input } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { forkJoin } from 'rxjs';
import { DistrictComparisionApiService } from 'src/app/model/district-comparision-api.service';
import { DistrictComparison, DistrictFiscalYearMetrics } from 'src/app/model/district-comparison';


@Component({
  selector: 'app-deficit-comparison-chart',
  templateUrl: './deficit-comparison.component.html',
  styleUrls: [ './deficit-comparison.component.css']
})
export class DeficitComparisonComponent implements OnInit {


  @Input() district: string;

  public barChartOptions: ChartOptions = {
     responsive: true,
     maintainAspectRatio: false,
     title: {
       display: true,
       text: 'Chichester Deficits',
       fontSize: 16
     },
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{
      scaleLabel: {
        display: true,
        labelString: 'Deficit'
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
  public comparisons: DistrictComparison;
  public condensed: boolean = true;

  constructor(
    private districtComparsionsApi: DistrictComparisionApiService) {
  }

  ngOnInit(): void {
    this.isBusy = true;

    forkJoin(
        this.districtComparsionsApi.getComparisons()
      )
      .subscribe(([comparisons]) => {
          this.comparisons = comparisons;

          //get the last two fiscal years 
          let keys = Object.keys(this.comparisons.fiscalYears).map(Number);

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

            this.addMetric(districtComparisonData, keys, 'Deficit', 'Blah'); 
          });


          this.isBusy = false;
        },
        error => console.error(error)
      );


  }


  addMetric(districtComparisonData: DistrictFiscalYearMetrics, fiscalYearKeys : number[], metricName: string, label: string) {

  let data = [];  

    //foreach fy metric set of data
    fiscalYearKeys.forEach(key => {
      //let fyMetrics = districtComparisonData.metricsByFiscalYear[key];
      
      //let metric = Math.round(fyMetrics..Deficit);
      //data.push(metric);

    });
 
    let districtData: ChartDataSets = { data: data, label: label };
    this.barChartData.push(districtData);
  }
}
