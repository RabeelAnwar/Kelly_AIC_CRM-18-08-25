// import { CanActivateFn, Router } from '@angular/router';
// import { AuthService } from '../services/auth.service';
// import { inject } from '@angular/core';

// export const authGuard: CanActivateFn = (route, state) => {
//   const logiin = inject(AuthService).isLoggedIn();
//   if (logiin) {
//     return true;
//   }
//   return inject(Router).createUrlTree(['/login']);
// };