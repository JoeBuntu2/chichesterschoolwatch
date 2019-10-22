import { Component } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  constructor(private router: Router) {

    //collapse menu when we navigate
    router.events.subscribe(e => {
      if(e instanceof NavigationStart) {
        this.collapse();
      }
    }); 
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  navigate(path: string) {
    this.router.navigate([path]);
  }
}
