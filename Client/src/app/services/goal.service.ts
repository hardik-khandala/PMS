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

  getAllGoals() {
    return this.http.get<Goal[]>(BaseUrl + '/api/Goal/GetAllGoals', getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  deleteGoal(id: number) {
    return this.http.delete<{ msg: string }>(BaseUrl + `/api/Goal/DeleteGoal/${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }
}
