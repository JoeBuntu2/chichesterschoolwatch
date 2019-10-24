import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-comparisons-grid',
  templateUrl: './comparisons-grid.component.html',
  styleUrls: ['./comparisons-grid.component.css']
})
export class ComparisonsGridComponent implements OnInit {

  @Input() metric: string;
  @Input() comparisons: any;

  public objectKeys = Object.keys;


  constructor() { }

  ngOnInit() {
  }

}
