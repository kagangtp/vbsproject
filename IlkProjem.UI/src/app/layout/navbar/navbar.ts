// layout/components/navbar/navbar.component.ts
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  userName: string = 'Misafir';
  userInitial: string = '?';

  ngOnInit() {
    this.loadUserData();
  }

  loadUserData() {
    const userJson = localStorage.getItem('currentUser');
    if (userJson) {
      const user = JSON.parse(userJson);
      // Örneğin: "Kagan Gündüztepe" -> "Kagan" kısmını alıyoruz
      this.userName = user.firstName || 'Kullanıcı';
      // İsmin ilk harfini ikon için alıyoruz
      this.userInitial = this.userName.charAt(0).toUpperCase();
    }
  }


  changeLanguage(lang: string) {
  // 1. Update the 'language' key in Local Storage
  localStorage.setItem('language', lang);

  // 2. Reload the page to ensure the Interceptor picks up the new header
  window.location.reload();
  }

}