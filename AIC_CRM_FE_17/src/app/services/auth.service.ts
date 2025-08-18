import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private dp: DatePipe,
    private router: Router,

  ) { }

  // logout(): Observable<any> {
  //   const dtNow = this.dp.transform(new Date(), 'yyyy/MM/dd HH:mm:ss');
  //   return this.http.post(`${environment.apiUrl}/auth/logout?dtNow=${dtNow}`, dtNow);
  // }


  logout() {
    localStorage.removeItem('userData');
    localStorage.removeItem('token');

    this.router.navigate(['/login']);
  }

  storeUserData(data: any) {
    localStorage.setItem('userData', JSON.stringify(data));
  }

  getTenantId(): number {
    const data = this.getUserData();
    if (data) { return data.tenantId; }
    return 0;
  }

  getUserFullName(): any {
    const data = this.getUserData();
    if (data) { return data.fullName; }
    return null;
  }

  getUserId(): any {
    const data = this.getUserData();
    if (data) { return data.userId; }
    return null;
  }

  getToken(): any {
    const data = this.getUserData();
    if (data) { return data.token; }
    return null;
  }

  // isLoggedIn(): boolean {
  //   return !!localStorage.getItem('token')
  // }

  private getUserData(): any {
    const userData = localStorage.getItem('userData');
    return userData ? JSON.parse(userData) : null;
  }
}