import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { Report } from '../models/report.model';
import { BaseUrl } from '../config/environment';
import { getHeaders } from '../config/headers';
import { catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getAllReport(pageNumber: number, pageSize: number, startDate: Date, endDate: Date, deptId: number | string) {
    return this.http.get<any>(BaseUrl + `/api/Report/GetAllReport/${pageNumber}?pageSize=${pageSize}&startDate=${startDate??''}&endDate=${endDate??''}&deptId=${deptId??''}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getPdf(id: number) {
    return this.http.get<{url: string, fileName: string}>(BaseUrl + `/api/Report/GenerateReport/${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }
}
