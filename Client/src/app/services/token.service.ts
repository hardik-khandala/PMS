import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }
  token = new BehaviorSubject<string | null>(localStorage.getItem('token'))  
}
