import { Inject, Injectable } from '@angular/core';
import { Router, NavigationEnd } from "@angular/router";
import { HttpClient } from '@angular/common/http'; 


@Injectable({
  providedIn: 'root'
})
export class PingerService {

  constructor(
    router: Router,
    private http: HttpClient, 
    @Inject('BASE_URL') baseUrl: string

  ) { 
      //poor mans google analytics
      router.events.subscribe(e => {
        if (e instanceof NavigationEnd) {

          //if ((<any>window).ga) {
          //  let tracker = (<any>window).ga.getAll()[0];
          //  if (tracker) {
          //    tracker('set', 'page', e.urlAfterRedirects);
          //    tracker('send', 'pageview');
          //  }
          //} 

        http.post(baseUrl + 'api/ping', null, {
              params: { url: e.urlAfterRedirects  }, 
          }).subscribe(x =>   {
            //console.log('success');
          });
        }
      }); 
  }
}
