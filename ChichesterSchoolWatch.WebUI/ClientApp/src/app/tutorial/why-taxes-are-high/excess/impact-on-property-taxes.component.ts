import { Component, OnInit, Inject } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-impact-on-property-taxes',
  templateUrl: './impact-on-property-taxes.component.html',
  styleUrls: ['./impact-on-property-taxes.component.css']
})
export class ImpactOnPropertyTaxesComponent  {

  public barChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
     title: {
       display: true,
       text: 'Chichester Property Taxes if Cost-Per-Student Matched Other Districts',
       fontSize: 16
     },
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [
      {
        scaleLabel: {
          display: true,
          labelString: 'Property Assessment Value'
        },
      }], yAxes: [{
      scaleLabel: {
        display: true,
        labelString: 'School Property Tax'
      },
      display: true,
      ticks: {
        beginAtZero: true,
        max: 12000
      }
    }] },
    plugins: {
      datalabels: {
        display: false
      }
    }
  };

  //property assessments
  public barChartLabels: Label[] = [
       '60,000',
       '80,000',
      '100,000',
      '120,000',
      '140,000',
      '160,000',
      '180,000',
      '200,000',
      '220,000',
      '240,000',
      '260,000',
      '280,000'
  ];
  public barChartType: ChartType = 'line';
  public barChartLegend = true;
  public barChartPlugins = [pluginDataLabels];

  public barChartData: ChartDataSets[] = [
    { label:  'Chichester', fill: false, data: [2391,3188,3986,4783,5580,6377,7174,7971,8768,9565,10363,11160]},
    { label:  'PennDelco', fill: false, data: [1431,1907,2384,2861,3338,3815,4292,4768,5245,5722,6199,6676]},
    { label:  'Interboro', fill: false, data: [1703,2270,2838,3406,3973,4541,5108,5676,6244,6811,7379,7946]},
    { label:  'GarnetValley', fill: false, data: [2226,2968,3710,4452,5194,5936,6678,7420,8162,8904,9646,10387]},
    { label:  'Ridley', fill: false, data: [1607,2142,2678,3213,3749,4285,4820,5356,5891,6427,6962,7498]},
    { label:  'SoutheastDelco', fill: false, data: [1617,2156,2696,3235,3774,4313,4852,5391,5930,6469,7009,7548]}
  ];
  public isBusy: boolean;
  public districts: any[];
  public comparisons: any;
  public condensed: boolean = true;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string) {

    this.isBusy = false;

  }
}
