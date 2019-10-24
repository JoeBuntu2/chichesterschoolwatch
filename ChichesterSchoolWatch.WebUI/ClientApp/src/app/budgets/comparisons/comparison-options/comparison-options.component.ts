import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-comparison-options',
  templateUrl: './comparison-options.component.html',
  styleUrls: ['./comparison-options.component.css']
})
export class ComparisonOptionsComponent implements OnInit {

  @Input() optionGroups: string;
 
  ngOnInit() {
  }

}
