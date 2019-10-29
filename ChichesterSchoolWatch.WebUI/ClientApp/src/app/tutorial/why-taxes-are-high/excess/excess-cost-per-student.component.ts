import { Component, OnInit, Inject } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-excess-cost-per-student',
  templateUrl: './excess-cost-per-student.component.html',
  styleUrls: ['./excess-cost-per-student.component.css']
})
export class ExcessCostPerStudentComponent {

  public barChartOptions: ChartOptions = {
    // responsive: true,

    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{}] },
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
        http.get<any>(baseUrl + 'api/DistrictComparisons'),
        http.get<any[]>(baseUrl + 'api/Districts')
      )
      .subscribe(([comparisons, districts]) => {
          this.comparisons = comparisons;
          this.districts = districts.sort((a, b) => a.name.localeCompare(b.name));

          const keys = Object.keys(this.comparisons.fiscalYears);

          //load fiscal years
          this.barChartLabels = [];
          keys.forEach(key => {
            var fy = this.comparisons.fiscalYears[key];
            this.barChartLabels.push(<Label> fy);
          });

          //foreach district set of data
          comparisons.districtFiscalYearMetrics.forEach(districtComparisonData => {

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
