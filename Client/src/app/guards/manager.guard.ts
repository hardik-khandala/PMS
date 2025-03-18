import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const managerGuard: CanActivateFn = (route, state) => {
  let auth = inject(AuthService)
  if(auth.roleMethod()=="Manager"){
    return true;
  }
  return inject(Router).createUrlTree(['login'])

};
