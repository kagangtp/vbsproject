import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/authService';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-loginpage',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './loginpage.html',
  styleUrl: './loginpage.css',
})
export class Loginpage {
    private router = inject(Router);
    private authService = inject(AuthService);

    loginData = { email: '', password: '' };
  
  onLogin() {
    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        // 1. Güvenlik anahtarını (Token) kaydet
        localStorage.setItem('token', response.token);
        
        // 2. Kullanıcı bilgilerini kaydet (İsim, Soyisim vb.)
        // Backend'den 'user' adında bir nesne geldiğini varsayıyoruz. 
        // LocalStorage sadece metin kabul ettiği için JSON.stringify kullanıyoruz.
        if (response.user) {
          localStorage.setItem('currentUser', JSON.stringify(response.user));
        }
        
        // 3. Başarılıysa yönlendir
        this.router.navigate(['/mainpage/dashboard']);
      },
      error: (err) => {
        // Hata mesajı yönetimi
        const msg = err.error?.message || "Bir hata oluştu"; 
        alert("Giriş başarısız: " + msg); 
      }
    });
  }
}