import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms'
import { AuthService } from '../../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Login } from '../../models/login.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm = new FormGroup({
    UserName: new FormControl('', Validators.required),
    PasswordHash: new FormControl('', Validators.required)
  })

  flag: boolean = false;
  errorMessage!: string;
  constructor(private auth: AuthService, private route: Router) { }
  login() {
    if (this.loginForm.valid) {
      this.auth.loginHandler(this.loginForm.value as Login).subscribe({
        next: (value) => {
          localStorage.setItem('token', value.token);
          this.route.navigateByUrl('dashboard');
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.error; 
        },
      })
    }
    else {
      this.flag = true;
    }
  }
}
