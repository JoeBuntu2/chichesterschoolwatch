import { Injectable } from '@angular/core';
import { District } from './district';
import { DistrictMetrics } from './district-comparison';

@Injectable({
  providedIn: 'root'
})
export class DistrictNameSorterService {

  constructor() { }

  public sort(a : District, b :District) : number {
    return a.name.localeCompare(b.name)
  }
}
