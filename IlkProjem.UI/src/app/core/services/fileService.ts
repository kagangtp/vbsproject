import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class FileService {
    private http = inject(HttpClient);
    private apiUrl = 'http://localhost:5005/api/Files';

    upload(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.post<any>(`${this.apiUrl}/upload`, formData);
    }

    assignOwner(fileId: string, ownerId: number, ownerType: string): Observable<any> {
        return this.http.put<any>(`${this.apiUrl}/${fileId}/assign`, { ownerId, ownerType });
    }
}
