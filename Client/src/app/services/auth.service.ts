import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseUrl } from '../config/environment';
import { catchError, tap, throwError } from 'rxjs';
import { Login } from '../models/login.model';
import { Router } from '@angular/router';
import { jwtDecode } from "jwt-decode";
import { getHeaders } from '../config/headers';
import { TokenService } from './token.service';
import { NotificationService } from './notification.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  role!: string
  constructor(private http: HttpClient, private route: Router, private tokenService: TokenService, private ns: NotificationService) { }
  loginHandler(login: Login) {
    return this.http.post<{ token: string }>(BaseUrl + '/api/auth/Login', login)
      .pipe(
        tap((data) => {
          this.tokenService.token.next(data.token)
          const decode = jwtDecode(data.token)
          Object.entries(decode).forEach(x => {
            if (x[0] == 'username') {
              localStorage.setItem('username', x[1]);
            }
          })
          this.ns.startConnection();
        }),
        catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    this.tokenService.token.next(null);
    this.ns.stopConnection();
    this.route.navigate(['']);
  }

  register(data: any) {
    return this.http.post<{ msg: string }>(BaseUrl + '/api/auth/Register', data, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  roleMethod() {
    const token = localStorage.getItem('token');
    if (token != null) {
      const decoded = jwtDecode(token);
      Object.entries(decoded).forEach(x => {
        if (x[0] == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role") {
          this.role = x[1];
        }
      });
    }
    return this.role;
  }
  isJwtExpired() {
    const token = localStorage.getItem('token');
    if (typeof (token) !== 'string' || !token) throw new Error('Invalid token provided');

    let isJwtExpired = false;
    const { exp } = jwtDecode(token);
    const currentTime = new Date().getTime() / 1000;

    if (currentTime > exp!) isJwtExpired = true;

    return isJwtExpired;
  }
}
