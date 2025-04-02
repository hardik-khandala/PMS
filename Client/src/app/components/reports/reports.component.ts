import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { Report } from '../../models/report.model';
import { ReportService } from '../../services/report.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { DataService } from '../../services/data.service';
import { FormsModule } from '@angular/forms';
import { json2csv } from 'json-2-csv'
import { Department } from '../../models/data.model';

@Component({
  selector: 'app-reports',
  imports: [NavbarComponent, CommonModule, FormsModule],
  templateUrl: './reports.component.html',
  styleUrl: './reports.component.css'
})
export class ReportsComponent {
  reviewList!: Report[];
  departments!: Department[];
  deptId: number | string = '';
  startDate!: Date;
  endDate!: Date;
  isEndDateInvalid: boolean = false;
  pageNumber: number = 1;
  totalPage!: number;
  pageSize: number = 5;

  constructor(private reportService: ReportService, private toastr: ToastrService, private ds: DataService) {
    this.getAllReport();
    this.getDepartment();
  }

  getAllReport() {
    this.reportService.getAllReport(this.pageNumber, this.pageSize, this.startDate, this.endDate, this.deptId).subscribe({
      next: (value) => {
        this.reviewList = value.data;
        this.pageNumber = value.pageNumber;
        this.totalPage = value.totalPage;
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    });
  }

  getDepartment() {
    this.ds.getAllDept().subscribe({
      next: (data) => {
        this.departments = data;
      },
      error: (err: HttpErrorResponse) => {
        console.log(err.error);
      }
    })
  }

  downloadData() {
    const data = this.reviewList;
    const csvData = json2csv(data as unknown as object[])
    let blob = new Blob([csvData], { type: 'application/vnd.ms-excel' })
    let url = URL.createObjectURL(blob);
    let a = document.createElement('a');
    a.href = url;
    a.download = Date.now().toString();
    a.click();
  }

  downloadEmpData(index: number) {
    const data = this.reviewList[index]
    const csvData = json2csv(data as unknown as object[])
    let blob = new Blob([csvData], { type: 'application/vnd.ms-excel' })
    let url = URL.createObjectURL(blob);
    let a = document.createElement('a');
    a.href = url;
    a.download = Date.now().toString();
    a.click();
  }
  downloadEmpDataPdf(id: number) {
    this.reportService.getPdf(id).subscribe({
      next: (data) => {
          let a = document.createElement('a');
          a.href = data.url;
          a.download = data.fileName;
          a.click();
        
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    })
  }

  onStartDateChange() {
    if (this.endDate && new Date(this.endDate) <= new Date(this.startDate)) {
      this.isEndDateInvalid = true;  // Mark End Date as invalid
    } else {
      this.isEndDateInvalid = false;  // Valid End Date
    }
    this.pageNumber = 1;
    this.getAllReport();
  }

  onEndDateChange() {
    if (this.startDate && new Date(this.endDate) <= new Date(this.startDate)) {
      this.isEndDateInvalid = true;  // Mark End Date as invalid
    } else {
      this.isEndDateInvalid = false;  // Valid End Date
    }
    this.pageNumber = 1;
    this.getAllReport();
  }

  onDeptChange() {
    this.pageNumber = 1;
    this.getAllReport();
  }

  nextPage() {
    this.pageNumber++;
    this.getAllReport();
  }

  previousPage() {
    this.pageNumber--;
    this.getAllReport();
  }

  onPageSizeChange() {
    this.pageNumber = 1;
    this.getAllReport();
  }
}
