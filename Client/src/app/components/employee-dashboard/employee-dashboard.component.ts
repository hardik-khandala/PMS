import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { EmployeeService } from '../../services/employee.service';
import { Employee } from '../../models/employee.model';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DataService } from '../../services/data.service';
import { FormsModule } from '@angular/forms';
import { DashboardData, Department } from '../../models/data.model';
import { BaseChartDirective } from 'ng2-charts';
import { ChartOptions } from 'chart.js';

@Component({
  selector: 'app-employee-dashboard',
  imports: [NavbarComponent, CommonModule, RouterModule, FormsModule, BaseChartDirective],
  templateUrl: './employee-dashboard.component.html',
  styleUrl: './employee-dashboard.component.css'
})
export class EmployeeDashboardComponent implements OnInit {
  employees!: Employee[];
  departments!: Department[]
  errorMessage!: string;
  pageNumber: number = 1;
  totalPage!: number;
  pageSize: number = 5;
  isOpen: boolean = false;
  id!: number;
  orderBy: string = '';


  search!: string;
  deptId: number | string = '';


  dashboardData: DashboardData = { totalEmployee: 0, totalReview: 0, totalGoal: 0, totalReport: 0 };
  constructor(private employeeService: EmployeeService, public authService: AuthService, private toastr: ToastrService, private router: Router, private ds: DataService) { }

  ngOnInit(): void {
    this.getEmployee()
    this.getDashBoardData()
    this.getDepartment()
  }
  public pieChartLabels = [];
  public pieChartDatasets = [{
    data: [this.dashboardData.totalEmployee, this.dashboardData.totalReview, this.dashboardData.totalGoal, this.dashboardData.totalReport]
  }];
  public pieChartLegend = true;
  public pieChartPlugins = [];
  public pieChartOptions: ChartOptions = {
    plugins: {
      legend: {
        display: true,
        position: 'right',
        labels: {
          boxWidth: 20,
          padding: 10,
        },
      },
    },
  };

  getEmployee() {
    this.employeeService.getAllEmployee(this.pageNumber, this.pageSize, this.search, this.deptId, this.orderBy).subscribe({
      next: (employee) => {
        this.employees = employee.data;
        this.totalPage = employee.totalPage;
        this.pageNumber = employee.pageNumber;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.error;
      }
    });
  }

  getDashBoardData() {
    this.ds.getDashboardData().subscribe({
      next: (data) => {
        this.dashboardData = data;
        this.pieChartDatasets = [{
          data: [
            this.dashboardData.totalEmployee,
            this.dashboardData.totalReview,
            this.dashboardData.totalGoal,
            this.dashboardData.totalReport
          ]
        }];
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    });
  }

  getDepartment() {
    this.ds.getAllDept().subscribe({
      next: (value) => {
        this.departments = value
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error")
      }
    })
  }

  editEmp(id: number) {
    this.router.navigate(['employee/edit', id])
  }
  deleteEmp() {
    this.employeeService.deleteEmployee(this.id).subscribe({
      next: (msg) => {
        this.toastr.warning(msg.msg, "Success");
        this.isOpen = false;
        this.pageNumber = 1;
        this.getEmployee();
        this.getDashBoardData();
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    })
  }

  onSearch() {
    this.pageNumber = 1;
    this.getEmployee();
  }

  onDeptChange() {
    this.pageNumber = 1;
    this.getEmployee();
  }

  openModel(id: number) {
    this.isOpen = true;
    this.id = id;
  }
  closeModal() {
    this.isOpen = false;
  }

  nextPage() {
    this.pageNumber++;
    this.getEmployee();
  }

  previousPage() {
    this.pageNumber--;
    this.getEmployee();
  }

  onPageSizeChange() {
    this.pageNumber = 1;
    this.getEmployee();
  }

  sortName() {
    if (this.orderBy == 'asc') {
      this.orderBy = 'dsc';
    }
    else if (this.orderBy == 'dsc') {
      this.orderBy = ''
    } else {
      this.orderBy = 'asc'
    }
    this.getEmployee()
  }
}
