import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NavbarComponent } from "../navbar/navbar.component";
import { GoalService } from '../../services/goal.service';
import { ToastrService } from 'ngx-toastr';
import { Goal } from '../../models/goal.model';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-goal',
  imports: [CommonModule, ReactiveFormsModule, NavbarComponent],
  templateUrl: './add-goal.component.html',
  styleUrl: './add-goal.component.css'
})
export class AddGoalComponent {
  goalForm = new FormGroup({
    title: new FormControl('', Validators.required),
    detail: new FormControl('', Validators.required),
    statusId: new FormControl('', Validators.required),
    dueDate: new FormControl('', Validators.required)
  })
  flag: boolean = false;
  isEditing: boolean = false;
  goalId!: number;
  todayDate!: string;

  constructor(private goalService: GoalService, private toastr: ToastrService, private activatedRoute: ActivatedRoute) {
    const today = new Date();
    this.todayDate = today.toISOString().split('T')[0];
    this.goalId = parseInt(this.activatedRoute.snapshot.paramMap.get('id')!)
    if (this.goalId) {
      goalService.getGoal(this.goalId).subscribe({
        next: (value) => {
          this.isEditing = true;
          this.goalForm.patchValue(value);
        },
        error: (err: HttpErrorResponse) => {
          toastr.error(err.error, "Error");
        }
      });
    }
  }

  submit() {
    if (this.isEditing) {
      if (this.goalForm.valid) {
        this.goalService.editGoal(this.goalId, this.goalForm.value as Goal).subscribe({
          next: (value) => {
            this.toastr.success(value.msg, "Success");
            this.goalForm.reset();
          },
          error: (err: HttpErrorResponse) => {
            this.toastr.success(err.error, "Error");
          }
        })
      }
      else {
        this.flag = true;
      }
    }
    else {
      if (this.goalForm.valid) {
        this.goalService.addGoal(this.goalForm.value as Goal).subscribe({
          next: (value) => {
            this.toastr.success(value.msg, "Success");
          },
          error: (err: HttpErrorResponse) => {
            this.toastr.error(err.error, "Error");
          }
        });
        this.goalForm.reset();
        this.flag = false;
      }
      else {
        this.flag = true;
      }
    }
  }
}
