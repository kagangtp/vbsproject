import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router'; // Router eklendi
import { AuthService } from '../../core/services/authService'; 
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-sidebar',
  standalone: true, // Standalone olduğunu belirtmeyi unutma
  imports: [TranslateModule, RouterModule], // Şablon (HTML) için modül
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  private authService = inject(AuthService);
  
  // Hata buradaydı: RouterModule yerine Router inject edilmeli
  private router = inject(Router); 

  onLogout() {
    // 1. Servisteki temizlik operasyonunu başlat (Hem Local hem Session Storage temizlenir)
    this.authService.logout();
    
    // 2. Kullanıcıyı login sayfasına yönlendir
    // Artık 'navigate' metodu Router üzerinde tanımlı olduğu için hata vermez
    this.router.navigate(['/']);
    
    console.log("Kullanıcı güvenli bir şekilde çıkış yaptı.");
  }
}