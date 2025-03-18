import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { EmployeeService } from '../../services/employee.service';
import { Employee } from '../../models/employee.model';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-employee-dashboard',
  imports: [NavbarComponent, CommonModule, RouterModule],
  templateUrl: './employee-dashboard.component.html',
  styleUrl: './employee-dashboard.component.css'
})
export class EmployeeDashboardComponent implements OnInit {
  employees!: Employee[];
  errorMessage!: string;
  pageNumber: number = 1;
  totalPage!: number
  constructor(private employeeService: EmployeeService, public authService: AuthService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.getEmployee()
  }

  getEmployee() {
    this.employeeService.getAllEmployee(this.pageNumber).subscribe({
      next: (employee) => {
        this.employees = employee.data;
        this.totalPage = employee.totalPage;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.error;
      }
    });
  }
  editEmp(id: number) {
    this.router.navigate(['employee/edit',id])
  }
  deleteEmp(id: number) {
    this.employeeService.deleteEmployee(id).subscribe({
      next: (msg) => {
        this.toastr.warning(msg.msg, "Success");
        this.getEmployee();
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    })
  }

  nextPage() {
    this.pageNumber++;
    this.getEmployee();
  }

  previousPage() {
    this.pageNumber--;
    this.getEmployee();
  }
}
