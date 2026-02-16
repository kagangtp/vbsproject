import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const token = localStorage.getItem('token');

  // Token varsa geçişe izin ver
  if (token) {
    return true;
  }

  // Token yoksa girişe yönlendir ve geçişi engelle
  console.warn('Yetkisiz erişim engellendi, Login sayfasına yönlendiriliyorsunuz...');
  router.navigate(['/login']);
  return false;
};