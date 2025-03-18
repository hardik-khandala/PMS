import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseUrl } from '../config/environment';
import { getHeaders } from '../config/headers';
import { Department } from '../models/department.model';
import { catchError, throwError } from 'rxjs';
import { Role } from '../models/role.model';
import { Criteria } from '../models/criteria.model';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private http: HttpClient, private tokenService: TokenService) { }

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
}
