import { Component, inject } from '@angular/core';
import { AuthService } from '../../core/services/authService';
import { TranslateModule } from '@ngx-translate/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  imports: [TranslateModule,RouterModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  private authService = inject(AuthService);

  onLogout() {
    // Servis içinde yazdığımız logout metodunu çağırıyoruz
    // Bu metot hem token'ı silecek hem de yönlendirme yapacak
    this.authService.logout();
  }

}
