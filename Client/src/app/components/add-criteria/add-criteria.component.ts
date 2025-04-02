import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DataService } from '../../services/data.service';
import { Criteria } from '../../models/data.model';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-criteria',
  imports: [NavbarComponent, CommonModule, ReactiveFormsModule],
  templateUrl: './add-criteria.component.html',
  styleUrl: './add-criteria.component.css'
})
export class AddCriteriaComponent {
  criteriaForm = new FormGroup({
    criteriaName: new FormControl('', Validators.required),
    details: new FormControl('', Validators.required)
  })

  flag: boolean = false;
  isEditing: boolean = false;

  constructor(private ds: DataService, private toastr: ToastrService) { }

  submit() {
    if (this.criteriaForm.valid) {
      this.ds.addCriteria(this.criteriaForm.value as Criteria).subscribe({
        next: (value) => {
          this.toastr.success(value.msg, "Success");
        },
        error: (err: HttpErrorResponse) => {
          this.toastr.error(err.error, "Error");
        }
      });
      this.criteriaForm.reset();
      this.flag = false;
    }
    else {
      this.flag = true;
    }
  }
}

