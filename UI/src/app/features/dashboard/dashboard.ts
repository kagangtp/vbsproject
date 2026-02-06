import { Component, inject, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common'; 
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms'; // 1. Bunları ekle
import { CustomerService } from '../../core/services/customerService';
import { Customer } from '../../core/models/customer';

@Component({
  selector: 'app-dashboard',
  standalone: true, // Standalone component yapısı
  imports: [CommonModule, CurrencyPipe, DatePipe, ReactiveFormsModule], // 2. ReactiveFormsModule mutlaka burada olmalı
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  customerService = inject(CustomerService);
  private fb = inject(FormBuilder); // 3. FormBuilder'ı inject et

  customers: Customer[] = [];
  updateForm!: FormGroup; // 4. Hata veren değişken tam olarak bu
  isModalOpen = false; 
  selectedCustomerId: number | null = null;

  ngOnInit() {
    this.loadCustomers();
    this.initUpdateForm(); // Formu başlat
  }

  // HTML'deki [formGroup]="updateForm" ile eşleşen yapı
  initUpdateForm() {
    this.updateForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      balance: [0, [Validators.required]]
    });
  }

  loadCustomers() {
    this.customerService.getCustomers().subscribe(data => {
      this.customers = data;
    });
  }

  onEdit(customer: Customer) {
    this.selectedCustomerId = customer.id;
    this.isModalOpen = true;
    
    // Formun içine verileri basıyoruz
    this.updateForm.patchValue({
      name: customer.name,
      email: customer.email,
      balance: customer.balance
    });
  }

  closeModal() {
    this.isModalOpen = false;
    this.updateForm.reset();
  }

  onUpdate() {
    if (this.updateForm.valid && this.selectedCustomerId) {
      const updatedCustomer = { 
        id: this.selectedCustomerId, 
        ...this.updateForm.value 
      };

      this.customerService.updateCustomer(this.selectedCustomerId, updatedCustomer).subscribe({
        next: () => {
          this.closeModal();
          this.loadCustomers(); 
        },
        error: (err) => alert('Güncelleme hatası: ' + err.message)
      });
    }
  }

  onDelete(id: number) {
    const isConfirmed = confirm('Emin misiniz?');
    if (isConfirmed) {
      this.customerService.deleteCustomer(id).subscribe(() => this.loadCustomers());
    }
  }

  isAddModalOpen = false;


  onAdd() {
  this.isAddModalOpen = true;
  this.updateForm.reset({
    name: '',
    email: '',
    balance: 0 // <--- Bunu eklemezsen form 'invalid' kalır!
  });
}



// "Kaydet" (Create) işlemi
onCreate() {
  if (this.updateForm.valid) {
    // Sadece name ve email'i alıyoruz, balance göndermiyoruz
    const { name, email } = this.updateForm.value;
    const newCustomer = { name, email }; 

    this.customerService.createCustomer(newCustomer).subscribe({
      next: () => {
        this.isAddModalOpen = false;
        this.loadCustomers(); // Listeyi yenile
      },
      error: (err) => {
        // Eğer hala parse error alıyorsan, buradaki mantığı düzelteceğiz
        console.error("Ekleme hatası:", err);
      }
    });
  }
}
}