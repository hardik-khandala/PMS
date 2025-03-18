import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DataService } from '../../services/data.service';
import { Department } from '../../models/department.model';
import { HttpErrorResponse } from '@angular/common/http';
import { Role } from '../../models/role.model';
import { ManagerList } from '../../models/managerList.model';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { EmployeeService } from '../../services/employee.service';
import { Employee } from '../../models/employee.model';

@Component({
  selector: 'app-register',
  imports: [NavbarComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm = new FormGroup({
    empName: new FormControl('', Validators.required),
    userName: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),
    passwordHash: new FormControl('', Validators.required),
    deptId: new FormControl('', Validators.required),
    roleId: new FormControl('', Validators.required),
    managerId: new FormControl('', Validators.required)
  })
  departments!: Department[]
  roles!: Role[]
  managers!: ManagerList[]
  flag: boolean = false;
  empId!: number;
  isEditing: boolean = false;

  constructor(private dataService: DataService, private authService: AuthService, private route: Router, private toast: ToastrService, private activatedRoute: ActivatedRoute, private employeeService: EmployeeService) {
    this.getDepartment();
    this.getRole();
    this.getManager();
    this.empId = parseInt(this.activatedRoute.snapshot.paramMap.get('id')!)
    if (this.empId) {
      employeeService.getEmployee(this.empId).subscribe({
        next: (data) => {
          this.isEditing = true;
          this.registerForm.patchValue(data);
        }
      })
    }
  }

  getDepartment() {
    this.dataService.getAllDept().subscribe({
      next: (data) => {
        this.departments = data;
      },
      error: (err: HttpErrorResponse) => {
        console.log(err.error);
      }
    })
  }

  getRole() {
    this.dataService.getAllRole().subscribe({
      next: (data) => {
        this.roles = data;
      },
      error: (err: HttpErrorResponse) => {
        console.log(err.error);
      }
    })
  }

  getManager() {
    this.dataService.getAllManager().subscribe({
      next: (data) => {
        this.managers = data;
      },
      error: (err: HttpErrorResponse) => {
        console.log(err.error);
      }
    })
  }

  submit() {
    if (this.isEditing) {
      if (this.registerForm.valid) {
        this.employeeService.editEmployee(this.empId, this.registerForm.value as Employee).subscribe({
          next: (data) => {
            this.toast.success(data.msg, "Success");
          },
          error: (err: HttpErrorResponse) => {
            this.toast.error(err.error, "Error");
          }
        })
        this.registerForm.reset();
        this.isEditing = false;
        this.flag = false;
      }
      else {
        this.flag = true;
      }
    }
    else {
      if (this.registerForm.valid) {
        this.authService.register({ ...this.registerForm.value, deptId: parseInt(this.registerForm.value.deptId!), roleId: parseInt(this.registerForm.value.roleId!), managerId: parseInt(this.registerForm.value.managerId!) }).subscribe({
          next: (value) => {
            this.toast.success(value.msg, "Success")
            this.flag = false;
          },
          error: (err: HttpErrorResponse) => {
            this.toast.error(err.error, "Error");
          },
        })
        this.registerForm.reset();
        this.flag = false;
      } else {
        this.flag = true;
      }
    }
  }
}
