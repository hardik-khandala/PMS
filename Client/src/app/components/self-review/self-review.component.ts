import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReviewService } from '../../services/review.service';
import { HttpErrorResponse } from '@angular/common/http';
import { DataService } from '../../services/data.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { SelfReview } from '../../models/review.model';
import { Criteria } from '../../models/data.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-self-review',
  imports: [NavbarComponent, CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './self-review.component.html',
  styleUrl: './self-review.component.css'
})
export class SelfReviewComponent {
  selfReviewForm = new FormGroup({
    startDate: new FormControl('', Validators.required),
    endDate: new FormControl('', Validators.required),
    selfRating: new FormControl('', Validators.required),
    strength: new FormControl('', Validators.required),
    improvement: new FormControl('', Validators.required)
  })
  criterias!: Criteria[];
  criteriaId!: number;
  criteriaName!: string;
  errorMessage!: string;
  flag: boolean = false;
  isEditing: boolean = false;
  isEndDateInvalid: boolean = false;

  editId!: number

  constructor(private reviewService: ReviewService, private dataService: DataService, private toastr: ToastrService, private route: ActivatedRoute, private router: Router, public authService: AuthService) {
    this.editId = parseInt(this.route.snapshot.paramMap.get('id')!)
    if (this.editId) {
      reviewService.getSelfReview(this.editId).subscribe({
        next: (data) => {
          this.criteriaId = data.criteriaId;
          this.isEditing = true;
          this.selfReviewForm.patchValue(data)
        },
        error: (err: HttpErrorResponse) => {
          toastr.error(err.error, "Error");
        }
      })
    }
    else {
      this.getCriteria();
    }
  }

  getCriteria() {
    this.dataService.getCriteria().subscribe({
      next: (data) => {
        this.criterias = data;
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    });
  }

  selectedOps(data: Criteria) {
    this.criteriaId = data.criteriaId;
    this.criteriaName = data.criteriaName;
  }

  submit() {
    if (this.isEditing) {
      if (this.selfReviewForm.valid) {
        this.reviewService.editSelfReview(this.editId, this.selfReviewForm.value as SelfReview).subscribe({
          next: (data) => {
            this.toastr.success(data.msg, "Success");
          },
          error: (err: HttpErrorResponse) => {
            this.toastr.error(err.error, "Error");
          }
        });
        this.isEditing = false;
        this.flag = false;
        this.selfReviewForm.reset()
      }
      else {
        this.flag = true;
      }
    }
    else {
      if (this.selfReviewForm.valid) {
        const data: SelfReview = {
          ...this.selfReviewForm.value as SelfReview,
          criteriaId: this.criteriaId
        }
        this.reviewService.selfRaview(data).subscribe({
          next: (value) => {
            this.toastr.success(value.msg, "Success");
          },
          error: (err: HttpErrorResponse) => {
            this.errorMessage = err.error
          },
        });
        this.selfReviewForm.reset();
        this.flag = false;
      }
      else {
        this.flag = true;
      }
    }

  }
  onStartDateChange() {
    if (this.selfReviewForm.get('endDate')?.value && new Date(this.selfReviewForm.get('endDate')?.value as any) <= new Date(this.selfReviewForm.get('endDate')?.value as any)) {
      this.isEndDateInvalid = true;  // Mark End Date as invalid
    } else {
      this.isEndDateInvalid = false;  // Valid End Date
    }
  }

  addCriteria() {

  }

}
