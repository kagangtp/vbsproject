// layout/components/navbar/navbar.component.ts
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  changeLanguage(lang: string) {
  // 1. Update the 'language' key in Local Storage
  localStorage.setItem('language', lang);

  // 2. Reload the page to ensure the Interceptor picks up the new header
  window.location.reload();
}

}