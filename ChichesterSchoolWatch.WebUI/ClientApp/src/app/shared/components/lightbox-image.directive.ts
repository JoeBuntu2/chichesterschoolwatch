import { Directive, Input, HostListener, OnInit } from '@angular/core';
import { Lightbox } from 'ngx-lightbox';

@Directive({
  selector: '[appLightboxImage]'
})
export class LightboxImageDirective implements OnInit {
  private album : any;


  @HostListener('click') onClick() { 
    this.lightbox.open(this.album, 0);    
  }

  @Input('appLightboxImage') Source: string;
  @Input('appLightboxImageCaption') Caption: string;

  constructor( private lightbox: Lightbox) {
    
  }

  ngOnInit(): void {
    this.album = [ {
      src: this.Source,
      caption: this.Caption,
      thumb: this.Source

    }];
  }

}
