import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NavbarComponent } from "../navbar/navbar.component";
import { CommonModule } from '@angular/common';
import { Goal } from '../../models/goal.model';
import { GoalService } from '../../services/goal.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-goal',
  imports: [RouterModule, NavbarComponent,CommonModule, FormsModule],
  templateUrl: './goal.component.html',
  styleUrl: './goal.component.css'
})
export class GoalComponent {
  goalList!: Goal[]
  isOpen: boolean = false;
  id!: number;

  pageNumber: number = 1;
  totalPage!: number;
  pageSize: number = 5;
  search!: string;
  statusId: number | string = '';

  constructor(private goalService: GoalService, private toastr: ToastrService, private router: Router) { 
    this.getGoalList();
  }

  getGoalList() {
    this.goalService.getAllGoals(this.pageNumber, this.pageSize, this.search, this.statusId).subscribe({
      next: (value) => {
        this.pageNumber = value.pageNumber;
        this.totalPage = value.totalPage;
        this.goalList = value.data;
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Erros");
      }
    });
  }

  onSearch() {
    this.pageNumber = 1;
    this.getGoalList();
  }

  onStatusChange() {
    this.pageNumber = 1;
    this.getGoalList();
  }

  edit(id: number){
    this.router.navigate(['goals/edit',id])
  }
  delete(){
    this.goalService.deleteGoal(this.id).subscribe({
      next: (data) => {
        this.toastr.success(data.msg, "Success");
        this.getGoalList();
        this.isOpen = false;
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error, "Error");
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
    this.getGoalList();
  }

  previousPage() {
    this.pageNumber--;
    this.getGoalList();
  }

  onPageSizeChange() {
    this.pageNumber = 1;
    this.getGoalList();
  }
}
