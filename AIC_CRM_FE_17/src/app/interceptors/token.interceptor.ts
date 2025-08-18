import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CommonService } from '../services/common.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(
    private auth: AuthService,
    private toster: ToastrService,
    private router: Router,
  ) { }

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const token = this.auth.getToken();
    if (request.url.includes('/login')) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer Token` },
      });
    }

    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(request).pipe(
      catchError((err: any) => {
        if (err instanceof HttpErrorResponse) {
          if (err.status == 401) {
            // this.com.hideLoader();
            this.toster.warning('Token is expired, Login Again');
            this.router.navigate(['/login']);
          }
        }
        if (err.url.includes('/login')) {
          //   this.com.hideLoader();
          return throwError(() => this.toster.info(err.error.msg));
        }
        // this.com.hideLoader();
        return throwError(() => this.toster.info(err.error.text));
      })
    );
  }
}