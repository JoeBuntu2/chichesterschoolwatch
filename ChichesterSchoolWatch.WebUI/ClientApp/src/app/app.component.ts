import { Component } from "@angular/core"; 
import { PingerService } from './pinger.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  route: string;

  constructor(pinger: PingerService) {
 
  }
}
