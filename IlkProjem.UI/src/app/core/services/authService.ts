import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  // Backend'in Program.cs'deki portu
  private apiUrl = 'http://localhost:5005/api/auth'; 

  login(loginData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, loginData);
  }

  // Token'ı güvenli bir şekilde kaydetmek için
  saveToken(token: string) {
    localStorage.setItem('token', token);
  }
  logout() {
  // 1. Token'ı temizle
  localStorage.removeItem('token');
  
  // 2. Kullanıcıyı login sayfasına yönlendir
  this.router.navigate(['/login']);
  }
  
}