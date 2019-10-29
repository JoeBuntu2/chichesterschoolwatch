import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-intro-welcome',
  templateUrl: './intro-welcome.component.html',
  styleUrls: ['./intro-welcome.component.css']
})
export class IntroWelcomeComponent implements OnInit {
  public condensed: boolean = true;
  constructor() { }

  ngOnInit() {
  }

}
