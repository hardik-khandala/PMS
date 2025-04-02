import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseUrl } from '../config/environment';
import { getHeaders } from '../config/headers';
import { catchError, throwError } from 'rxjs';
import { TokenService } from './token.service';
import { Criteria, DashboardData, Department, Role } from '../models/data.model';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getDashboardData() {
    return this.http.get<DashboardData>(BaseUrl + '/api/Data/dashboardData', getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getAllDept() {
    return this.http.get<Department[]>(BaseUrl + '/api/Data/getAllDept', getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getAllRole() {
    return this.http.get<Role[]>(BaseUrl + '/api/Data/getAllRole', getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getAllManager() {
    return this.http.get<[]>(BaseUrl + '/api/Data/getAllManager', getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getCriteria() {
    return this.http.get<Criteria[]>(BaseUrl + '/api/Data/getAllCriteria', getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  addCriteria(criteria: Criteria) {
    return this.http.post<{ msg: string }>(BaseUrl + '/api/Data/AddCriteria', criteria, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }
}
