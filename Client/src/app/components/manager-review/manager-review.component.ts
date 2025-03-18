import { Component, OnInit } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { CommonModule } from '@angular/common';
import { ManagerReviewList } from '../../models/managerReviewList.model';
import { ReviewService } from '../../services/review.service';
import { HttpErrorResponse } from '@angular/common/http';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { managerReview } from '../../models/managerReview.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-manager-review',
  imports: [NavbarComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './manager-review.component.html',
  styleUrl: './manager-review.component.css'
})
export class ManagerReviewComponent {
  managerReview = new FormGroup({
    managerRating: new FormControl('', Validators.required),
    managerFeedback: new FormControl('', Validators.required)
  });
  List!: ManagerReviewList[]
  showModal: boolean = false;
  flag: boolean = false;
  id!: number;
  constructor(private reviewService: ReviewService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getData();
  }
  getData() {
    this.reviewService.managerReviewList().subscribe({
      next: (value) => {
        this.List = value;
      },
      error: (err: HttpErrorResponse) => {
        console.log(err.error);
      }
    });
  }

  openModal(id: number) {
    this.showModal = true;
    this.id = id;
  }
  closeModal() {
    this.showModal = false;
    this.flag = false;
  }
  submit() {
    if (this.managerReview.valid) {
      const data: managerReview = {
        ...this.managerReview.value as managerReview,
        reviewId: this.id,
      }
      this.reviewService.managerReview(data).subscribe({
        next: (value) => {
          this.toastr.success(value.msg, "Success");
          this.getData();
        },
        error: (err: HttpErrorResponse) => {
          this.toastr.error(err.error, "Error");
        }
      });
    }
    else {
      this.flag = true;
    }
    this.managerReview.reset();
    this.showModal = false;
  }
  revise(id: number) {
    this.reviewService.managerRevise(id).subscribe({
      next: (value) => {
        this.toastr.info(value.msg, "Info");
        this.getData(); 
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    });
  }

}
