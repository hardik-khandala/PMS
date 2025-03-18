import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReviewService } from '../../services/review.service';
import { SelfReviewList } from '../../models/selfReviewList.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-yourreview',
  imports: [NavbarComponent, RouterModule, CommonModule],
  templateUrl: './yourreview.component.html',
  styleUrl: './yourreview.component.css'
})
export class YourreviewComponent {
  reviewList!: SelfReviewList[]

  constructor(private reviewService: ReviewService, private toast: ToastrService, private route: Router) { }
  ngOnInit(): void {
    this.getData();
  }
  getData() {
    this.reviewService.selfReviewList().subscribe({
      next: (review) => {
        this.reviewList = review
      },
      error: (err: HttpErrorResponse) => {
        console.log(err.error);
      }
    })
  }

  edit(id: number) {
    this.route.navigate(['self-review/edit',id])
  }

  delete(id: number) {
    this.reviewService.selfReviewDelete(id).subscribe({
      next: (msg) => {
        this.toast.info(msg.msg, "Info");
        this.getData();
      }
    })
  }
}
