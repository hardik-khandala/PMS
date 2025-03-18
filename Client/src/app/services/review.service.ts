import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { SelfReview } from '../models/selfReview.model';
import { BaseUrl } from '../config/environment';
import { BehaviorSubject, catchError, throwError } from 'rxjs';
import { SelfReviewList } from '../models/selfReviewList.model';
import { getHeaders } from '../config/headers';
import { ManagerReviewList } from '../models/managerReviewList.model';
import { managerReview } from '../models/managerReview.model';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  constructor(private http: HttpClient, private tokenService: TokenService) { }


  selfReviewList() {
    return this.http.get<SelfReviewList[]>(BaseUrl + '/api/Review/SelfReviewList', getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  getSelfReview(id: number) {
    return this.http.get<SelfReview>(BaseUrl + `/api/Review/getReview?id=${id}`, getHeaders(this.tokenService.token.value))
      .pipe(catchError((err: HttpErrorResponse) => throwError(() => err)))
  }

  managerReviewList() {
    return this.http.get<ManagerReviewList[]>(BaseUrl + '/api/Review/managerReviewList', getHeaders(this.tokenService.token.value))
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

  managerRevise(id: number) {
    return this.http.put<{ msg: string }>(BaseUrl + `/api/Review/ManagerRevise/${id}`, {}, getHeaders(this.tokenService.token.value))
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
