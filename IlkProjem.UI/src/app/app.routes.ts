import { Routes } from '@angular/router';
import { Loginpage } from './layout/loginpage/loginpage';
import { Dashboard } from './features/dashboard/dashboard';
import { Mainpage } from './layout/mainpage/mainpage';
import { authGuard } from './core/guards/auth-guard';
import { RegistrationPage } from './layout/registrationpage/registrationpage';
import { loginGuard } from './core/guards/login-guard';

export const routes: Routes = [
  { path: 'register', component: RegistrationPage },

  { 
    path: 'login', 
    component: Loginpage, 
    canActivate: [loginGuard] // 👈 Bunu ekledik
  },

  {
    path: 'mainpage',
    component: Mainpage,
    canActivate: [authGuard], // 🔐 Ana kapıya kilidi vurduk; artık bypass imkansız!
    children: [
      { path: 'dashboard', component: Dashboard },
      
      // Buraya Customers, Accounts vb. eklediğinde onlar da otomatik korunur
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' }
];