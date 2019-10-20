import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StatsService {
  public chiAssessmentBoostFy1819: number;
  public chiDeficitFy1920: number;

  constructor() {
    this.chiAssessmentBoostFy1819 = 80443934;
    this.chiDeficitFy1920 = 3477090;
  }
}
