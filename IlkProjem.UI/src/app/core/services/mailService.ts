import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class MailService {
    private http = inject(HttpClient);
    private apiUrl = 'http://localhost:5005/api/Mail';

    send(mail: { to: string; subject: string; body: string }): Observable<any> {
        return this.http.post<any>(`${this.apiUrl}/send`, mail);
    }
}
