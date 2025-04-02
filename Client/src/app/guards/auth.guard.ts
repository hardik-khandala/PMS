import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  let auth = inject(AuthService);

  const allowedRoles = route.data?.['roles'] || [];

  if (allowedRoles.includes(auth.roleMethod()) && !auth.isJwtExpired()) {
    return true;
  }

  auth.logout();
  return false;
};
