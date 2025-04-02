import { Injectable, signal } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {
  private isLoadingSignal = signal<boolean>(false);

  constructor(private spinner: NgxSpinnerService) { }

  get isLoading() {
    return this.isLoadingSignal.asReadonly();
  }

  show(): void {
    this.spinner.show()
    this.isLoadingSignal.set(true);
  }

  hide(): void {
    this.spinner.hide()
    this.isLoadingSignal.set(false);
  }
}
