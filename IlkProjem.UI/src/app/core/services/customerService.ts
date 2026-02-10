import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // 1. Bunu ekle
import { Observable } from 'rxjs'; // 2. Bunu ekle
import { Customer } from '../models/customer'; // Interface yolun
import { CustomerUpdateDto } from '../models/customer-update-dto';
import { CustomerDeleteDto } from '../models/customer-delete-dto';
import { ListResponseModel } from '../models/responses/list-response-model';
import { SingleResponseModel } from '../models/responses/single-response-model';
import { ResponseModel } from '../models/responses/response-model';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private http = inject(HttpClient); // 3. Http bağımlılığını enjekte et
  private apiUrl = 'http://localhost:5005/api/Customer'; // 4. Backend portunu buraya yaz

  // "Getir" - Now expects the ListResponseModel wrapper
  getCustomers(): Observable<ListResponseModel<Customer>> {
    return this.http.get<ListResponseModel<Customer>>(this.apiUrl);
  }

  // "GetirById"
  getCustomerById(id: number): Observable<SingleResponseModel<Customer>> {
    return this.http.get<SingleResponseModel<Customer>>(`${this.apiUrl}/${id}`);
  }

  // "Güncelle" - Returns ResponseModel (success/message)
  updateCustomer(customer: CustomerUpdateDto): Observable<ResponseModel> {
    return this.http.put<ResponseModel>(this.apiUrl, customer);
  }

  // "Sil" - Uses the DeleteDto in the body
  deleteCustomer(id: number): Observable<ResponseModel> {
    const deleteDto: CustomerDeleteDto = { id: id };
    return this.http.delete<ResponseModel>(this.apiUrl, { body: deleteDto });
  }

  // "Ekle" - Returns ResponseModel
  createCustomer(customer: any): Observable<ResponseModel> {
    return this.http.post<ResponseModel>(this.apiUrl, customer);
  }
}