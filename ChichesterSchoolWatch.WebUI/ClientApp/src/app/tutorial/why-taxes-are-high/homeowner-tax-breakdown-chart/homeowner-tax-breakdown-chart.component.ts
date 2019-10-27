import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';


@Component({
  selector: 'app-homeowner-tax-breakdown-chart',
  templateUrl: './homeowner-tax-breakdown-chart.component.html',
  styleUrls: ['./homeowner-tax-breakdown-chart.component.css']
})
export class HomeownerTaxBreakdownChartComponent implements OnInit {

  public barChartOptions: ChartOptions = {
    responsive: true,
    legend: {
      display: false
    },
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public barChartLabels: Label[] = ['School', 'Township', 'County'];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [pluginDataLabels];

  public barChartData: ChartDataSets[] = [
    { data: [79, 10, 11] }
  ];


  ngOnInit() {
  }  
}
