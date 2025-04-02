import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { CommonModule } from '@angular/common';
import { ReviewService } from '../../services/review.service';
import { HttpErrorResponse } from '@angular/common/http';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { managerReview, ManagerReviewList } from '../../models/review.model';

@Component({
  selector: 'app-manager-review',
  imports: [NavbarComponent, CommonModule, ReactiveFormsModule, FormsModule],
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
  showModalRevise: boolean = false;
  flag: boolean = false;
  id!: number;
  length: number = 0;

  pageNumber: number = 1;
  totalPage!: number;
  pageSize: number = 5;
  search!: string;
  rating: number | string = '';
  constructor(private reviewService: ReviewService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getData();
  }
  getData() {
    this.reviewService.managerReviewList(this.pageNumber, this.pageSize, this.search, this.rating).subscribe({
      next: (value) => {
        this.List = value.data;
        this.pageNumber = value.pageNumber;
        this.totalPage = value.totalPage;
        this.length = this.List.length;
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
  openReviseModel(id: number){
    this.showModalRevise = true;
    this.id = id;
  }
  closeModal() {
    this.showModal = false;
    this.showModalRevise = false;
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
          this.showModal = false;
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
  }
  revise() {
    console.log(this.managerReview.get('managerFeedback')?.value);
    
    this.reviewService.managerRevise(this.id, this.managerReview.get('managerFeedback')?.value as string).subscribe({
      next: (value) => {
        this.toastr.info(value.msg, "Info");
        this.getData(); 
        this.closeModal();
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    });
  }

  onSearch() {
    this.pageNumber = 1;
    this.getData();
  }

  onStatusChange() {
    this.pageNumber = 1;
    this.getData();
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
