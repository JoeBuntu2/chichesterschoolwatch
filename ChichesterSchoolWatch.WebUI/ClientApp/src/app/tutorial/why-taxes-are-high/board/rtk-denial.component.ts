import { Component, OnInit } from '@angular/core';
import { Lightbox } from 'ngx-lightbox';

@Component({
  selector: 'app-rtk-denial',
  templateUrl: './rtk-denial.component.html',
  styleUrls: ['./rtk-denial.component.css']
})
export class RtkDenialComponent  {
  private album: any[] =  [ {
    src: "https://cdn.chichesterschoolwatch.com/rtk-denied.PNG",
    caption: "Denied",
    thumb: "https://cdn.chichesterschoolwatch.com/rtk-denied.PNG"

  }];

  constructor(
    private lightbox: Lightbox,) {

  }

  open(index: number): void {
    // open lightbox
    this.lightbox.open(this.album, index);
  }

}
