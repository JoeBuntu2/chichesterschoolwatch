import { Injectable, Inject} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { District } from './district';
import { Observable } from 'rxjs';
import { DistrictComparison } from './district-comparison';

@Injectable({
  providedIn: 'root'
})
export class DistrictComparisionApiService {

  constructor(
    private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string) 
    {

    }

  getComparisons() : Observable<DistrictComparison>{
    return this.http.get<DistrictComparison>(this.baseUrl + 'api/DistrictComparisons') 
  }
}
