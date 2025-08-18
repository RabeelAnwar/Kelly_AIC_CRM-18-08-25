import { environment } from './../../environment/environmemt';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root',
})
export class ApiService {

  constructor(private http: HttpClient) { }

  public getData(url: any): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/${url}`);
  }

  public getDataById(url: any, obj: {}): Observable<any> {
    const params = new HttpParams({ fromObject: obj });
    return this.http.get<any>(`${environment.apiUrl}/${url}?${params.toString()}`);
  }

   public getDataByName(url: any, obj: {}): Observable<any> {
    const params = new HttpParams({ fromObject: obj });
    return this.http.get<any>(`${environment.apiUrl}/${url}?${params.toString()}`);
  }

  saveObj(url: any, obj: any): Observable<any> {
    const params = new HttpParams({ fromObject: obj });
    return this.http.post(`${environment.apiUrl}/${url}?${params.toString()}`, obj)
  }

  saveData(url: any, obj: any): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/${url}`, obj);
  }

  save(url: any, parm: any, body: any): Observable<any> {
    const params = new HttpParams({ fromObject: parm });
    return this.http.post(`${environment.apiUrl}/${url}?${params.toString()}`, body)
  }

  saveFormData(url: string, formData: FormData): Observable<any> {
    return this.http.post(`${environment.apiUrl}/${url}`, formData);
  }

  deleteData(url: any, obj: {}): Observable<any> {
    const params = new HttpParams({ fromObject: obj });
    return this.http.delete(`${environment.apiUrl}/${url}?${params.toString()}`);
  }

}