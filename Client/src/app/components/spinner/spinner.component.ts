import { Component, computed, inject } from '@angular/core';
import { NgxSpinnerComponent } from 'ngx-spinner';
import { SpinnerService } from '../../services/spinner.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-spinner',
  imports: [NgxSpinnerComponent, CommonModule],
  templateUrl: './spinner.component.html',
  styleUrl: './spinner.component.css'
})
export class SpinnerComponent {
  service = inject(SpinnerService);
  isLoading = computed(() => this.service.isLoading())
}
