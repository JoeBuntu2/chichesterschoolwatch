import { Injectable, Inject} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { District } from './district';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DistrictApiService {

  constructor(
    private http: HttpClient, 
    @Inject('BASE_URL') private baseUrl: string) 
    {

    }

  getDistricts() : Observable<District[]>{
    return this.http.get<District[]>(this.baseUrl + 'api/Districts') 
  }
}
