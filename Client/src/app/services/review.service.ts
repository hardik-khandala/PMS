import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { managerReview, SelfReview } from '../models/review.model';
import { BaseUrl } from '../config/environment';
import { catchError, throwError } from 'rxjs';
import { getHeaders } from '../config/headers';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  constructor(private http: HttpClient, private tokenService: TokenService) { }

  selfReviewList(pageNumber: number, pageSize: number, name: string, statusId: number | string) {
    return this.http.get<any>(BaseUrl + `/api/Review/SelfReviewList/${pageNumber}?pageSize=${pageSize}&search=${name??''}&statusId=${statusId??''}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getSelfReview(id: number) {
    return this.http.get<SelfReview>(BaseUrl + `/api/Review/getReview?id=${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  managerReviewList(pageNumber: number, pageSize: number, name: string, rating: number | string) {
    return this.http.get<any>(BaseUrl + `/api/Review/managerReviewList/${pageNumber}?pageSize=${pageSize}&search=${name??''}&rating=${rating??''}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  selfRaview(review: SelfReview) {
    return this.http.post<{ msg: string }>(BaseUrl + '/api/Review/SelfReview', review, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  managerReview(managerReview: managerReview) {
    return this.http.put<{ msg: string }>(BaseUrl + '/api/Review/ManagerReview', managerReview, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  managerRevise(id: number, managerFeedback: string) {
    return this.http.put<{ msg: string }>(BaseUrl + `/api/Review/ManagerRevise/${id}?managerFeedback=${managerFeedback}`, {}, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  editSelfReview(id: number, review: SelfReview) {
    return this.http.put<{ msg: string }>(BaseUrl + `/api/Review/EditSelfReview/${id}`, review, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  selfReviewDelete(id: number) {
    return this.http.delete<{ msg: string }>(BaseUrl + `/api/Review/deleteSelfReview/${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }


}
