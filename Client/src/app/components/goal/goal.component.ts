import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from "../navbar/navbar.component";
import { CommonModule } from '@angular/common';
import { Goal } from '../../models/goal.model';
import { GoalService } from '../../services/goal.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-goal',
  imports: [RouterModule, NavbarComponent,CommonModule],
  templateUrl: './goal.component.html',
  styleUrl: './goal.component.css'
})
export class GoalComponent {
  goalList!: Goal[]

  constructor(private goalService: GoalService, private toastr: ToastrService) { 
    this.getGoalList();
  }

  getGoalList() {
    this.goalService.getAllGoals().subscribe({
      next: (value) => {
        console.log(value);
        
        this.goalList = value;
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Erros");
      }
    });
  }

  edit(id: number){

  }
  delete(id: number){
    this.goalService.deleteGoal(id).subscribe({
      next: (data) => {
        this.toastr.success(data.msg, "Success");
        this.getGoalList();
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
      }
    })
  }
}
