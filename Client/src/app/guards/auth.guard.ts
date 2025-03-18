import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  let auth = inject(AuthService)
  if(auth.roleMethod()=="Admin" || auth.roleMethod() == "Employee" || auth.roleMethod() == "Manager" || auth.roleMethod() == "HR"){
    return true;
  }
  return inject(Router).createUrlTree(['login'])
};
