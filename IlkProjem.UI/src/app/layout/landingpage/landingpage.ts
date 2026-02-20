import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [TranslateModule],
  templateUrl: './landingpage.html',
  styleUrls: ['./landingpage.css']
})
export class Landingpage {
  private router = inject(Router);
  goToDashboard() {
    this.router.navigate(['mainpage/dashboard']);
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }

  changeLanguage(lang: string) {
    localStorage.setItem('language', lang);
    window.location.reload();
  }
}