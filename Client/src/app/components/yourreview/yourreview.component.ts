import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReviewService } from '../../services/review.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { SelfReviewList } from '../../models/review.model';

@Component({
  selector: 'app-yourreview',
  imports: [NavbarComponent, RouterModule, CommonModule, FormsModule],
  templateUrl: './yourreview.component.html',
  styleUrl: './yourreview.component.css'
})
export class YourreviewComponent {
  reviewList!: SelfReviewList[]
  isOpen: boolean = false;
  id!: number;
  pageNumber: number = 1;
  totalPage!: number;
  pageSize: number = 5
  search!: string;
  statusId: number | string = '';
  length: number = 0;

  constructor(private reviewService: ReviewService, private toast: ToastrService, private route: Router) { }
  ngOnInit(): void {
    this.getData();
  }
  getData() {
    this.reviewService.selfReviewList(this.pageNumber, this.pageSize, this.search, this.statusId).subscribe({
      next: (review) => {
        this.reviewList = review.data;
        this.totalPage = review.totalPage;
        this.pageNumber = review.pageNumber
        this.length = this.reviewList.length;
      },
      error: (err: HttpErrorResponse) => {
        console.log(err.error);
      }
    })
  }

  onSearch() {
    this.pageNumber = 1;
    this.getData();
  }

  onStatusChange() {
    this.pageNumber = 1;
    this.getData();
  }

  edit(id: number) {
    this.route.navigate(['self-review/edit', id])
  }

  delete() {
    this.reviewService.selfReviewDelete(this.id).subscribe({
      next: (msg) => {
        this.toast.info(msg.msg, "Info");
        this.getData();
        this.isOpen = false;
      }
    })
  }

  openModel(id: number) {
    this.isOpen = true;
    this.id = id;
  }

  closeModel() {
    this.isOpen = false;
  }

  nextPage() {
    this.pageNumber++;
    this.getData();
  }

  previousPage() {
    this.pageNumber--;
    this.getData();
  }

  onPageSizeChange() {
    this.pageNumber = 1;
    this.getData();
  }
}
