import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // 1. Bunu ekle
import { Observable } from 'rxjs'; // 2. Bunu ekle
import { Customer } from '../models/customer'; // Interface yolun
import { CustomerUpdateDto } from '../models/customer-update-dto';
import { CustomerDeleteDto } from '../models/customer-delete-dto';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private http = inject(HttpClient); // 3. Http bağımlılığını enjekte et
  private apiUrl = 'http://localhost:5005/api'; // 4. Backend portunu buraya yaz

  getCustomers(): Observable<Customer[]> {
    return this.http.get<Customer[]>(`${this.apiUrl}/Customer`);
  }

  updateCustomer(id: number, customer: CustomerUpdateDto): Observable<any> {
  return this.http.put(`${this.apiUrl}/Customer`, customer,{responseType: 'text' as 'json'});
  }

  deleteCustomer(id: number): Observable<any> {
    const deleteDto: CustomerDeleteDto = { id: id }; // DTO objesini oluştur
    return this.http.delete(`${this.apiUrl}/Customer`, {body: deleteDto});
  }

  createCustomer(customer: any): Observable<any> {
  return this.http.post(`${this.apiUrl}/Customer`, customer, {responseType: 'text' as 'json'});
}
}