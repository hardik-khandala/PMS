import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseUrl } from '../config/environment';
import { catchError, throwError } from 'rxjs';
import { getHeaders } from '../config/headers';
import { TokenService } from './token.service';
import { Employee } from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getEmployee(id: number) {
    return this.http.get<Employee>(BaseUrl + `/api/Employee/getEmp/${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getAllEmployee(pageNumber: number, pageSize: number, name: string, deptId: number | string, orderBy: string) {
    return this.http.get<any>(BaseUrl + `/api/Employee/getEmployee/${pageNumber}?pageSize=${pageSize}&search=${name??''}&deptId=${deptId??''}&orderBy=${orderBy??''}`)
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  editEmployee(id: number, emp: Employee) {
    return this.http.put<{ msg: string }>(BaseUrl + `/api/Employee/editEmp/${id}`, emp, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  deleteEmployee(id: number) {
    return this.http.delete<{ msg: string }>(BaseUrl + `/api/Employee/deleteEmp/${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }
}
