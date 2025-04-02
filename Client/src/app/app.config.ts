import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations'
import { NgxSpinnerModule } from "ngx-spinner";
import { spinnerInterceptor } from './Interceptor/spinner.interceptor';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideHttpClient(withInterceptors([spinnerInterceptor])), provideAnimations(), provideToastr({ timeOut: 2000, positionClass: "toast-top-right", preventDuplicates: true, closeButton: true }), NgxSpinnerModule, provideCharts(withDefaultRegisterables())]
};
