import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { SingleResponseModel } from '../models/responses/single-response-model';
import { ResponseModel } from '../models/responses/response-model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  // Backend'in Program.cs'deki portu
  private apiUrl = 'http://localhost:5005/api/auth'; 


// authService.ts
login(loginData: any): Observable<SingleResponseModel<string>> {
  // Backend'den string (token) içeren bir SingleResponseModel bekliyoruz
  return this.http.post<SingleResponseModel<string>>(`${this.apiUrl}/login`, loginData);
}

register(registerData: any): Observable<ResponseModel> {
  return this.http.post<ResponseModel>(`${this.apiUrl}/register`, registerData);
}
  logout() {
  // Hem kalıcı hem geçici hafızayı süpürüyoruz
  localStorage.removeItem('token');
  sessionStorage.removeItem('token');
  
  // Eğer başka kullanıcı verileri tutuyorsan onları da burada silmelisin
  localStorage.removeItem('currentUser');
  sessionStorage.removeItem('currentUser');
}

  getToken(): string | null {
  return localStorage.getItem('token') || sessionStorage.getItem('token');
}

  saveToken(token: string, rememberMe: boolean = false) {
  if (rememberMe) {
    // Seçiliyse: Tarayıcı kapansa da gitmez
    localStorage.setItem('token', token);
  } else {
    // Seçili değilse: Sekme/Tarayıcı kapanınca silinir
    sessionStorage.setItem('token', token);
  }
}

isLoggedIn(): boolean {
  const token = this.getToken();
  // Burada istersen JWT Helper ile token süresi (expired) kontrolü de yapabilirsin
  return !!token;
}
  
}