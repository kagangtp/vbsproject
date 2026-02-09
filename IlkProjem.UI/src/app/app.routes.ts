import { Routes } from '@angular/router';
import { Loginpage } from './layout/loginpage/loginpage';
import { Dashboard } from './features/dashboard/dashboard';
import { Mainpage } from './layout/mainpage/mainpage';

export const routes: Routes = [
  { path: 'login', component: Loginpage },

  // 2. Durak: Ana Uygulama Grubu (Prefix: /mainpage)
  {
    path: 'mainpage',
    component: Mainpage, // Burası senin Navbar ve Sidebar'ının olduğu iskelet
    children: [
      // localhost/mainpage/dashboard
      { path: 'dashboard', component: Dashboard },
      
      // Sadece /mainpage yazılırsa otomatik dashboard'a at
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  // Varsayılan Yönlendirmeler
  { path: '', redirectTo: 'login', pathMatch: 'full' }, // Site açılınca login'e git
  { path: '**', redirectTo: 'login' } // Hatalı URL yazılırsa login'e at
];