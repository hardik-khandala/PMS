import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { Goal } from '../models/goal.model';
import { BaseUrl } from '../config/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { getHeaders } from '../config/headers';
import { catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GoalService {

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  getGoal(id: number) {
    return this.http.get<Goal>(BaseUrl + `/api/Goal/GetGoal/${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getAllGoals(pageNumber: number, pageSize: number, name: string, statusId: number | string) {
    return this.http.get<any>(BaseUrl + `/api/Goal/GetAllGoals/${pageNumber}?pageSize=${pageSize}&search=${name??''}&statusId=${statusId??''}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  addGoal(goal: Goal) {
    return this.http.post<{ msg: string }>(BaseUrl + `/api/Goal/AddGoal`, goal, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  editGoal(id: number, goal: Goal) {
    return this.http.put<{ msg: string }>(BaseUrl + `/api/Goal/EditGoal/${id}`, goal, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  deleteGoal(id: number) {
    return this.http.delete<{ msg: string }>(BaseUrl + `/api/Goal/DeleteGoal/${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }
}
