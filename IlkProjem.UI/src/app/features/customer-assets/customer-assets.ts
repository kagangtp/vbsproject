import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { CarService } from '../../core/services/carService';
import { HouseService } from '../../core/services/houseService';
import { CustomerService } from '../../core/services/customerService';
import { Car } from '../../core/models/car';
import { House } from '../../core/models/house';
import { Customer } from '../../core/models/customer';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    selector: 'app-customer-assets',
    standalone: true,
    imports: [CommonModule, TranslateModule],
    templateUrl: './customer-assets.html',
    styleUrl: './customer-assets.css',
})
export class CustomerAssets implements OnInit {
    private route = inject(ActivatedRoute);
    private router = inject(Router);
    private carService = inject(CarService);
    private houseService = inject(HouseService);
    private customerService = inject(CustomerService);

    customerId = 0;
    customer: Customer | null = null;
    cars: Car[] = [];
    houses: House[] = [];

    // Lightbox
    lightboxImage: string | null = null;
    lightboxAlt = '';

    ngOnInit() {
        this.customerId = +this.route.snapshot.params['id'];
        this.loadCustomer();
        this.loadCars();
        this.loadHouses();
    }

    loadCustomer() {
        this.customerService.getCustomerById(this.customerId).subscribe(res => {
            if (res.success) this.customer = res.data;
        });
    }

    loadCars() {
        this.carService.getCarsByCustomer(this.customerId).subscribe(res => {
            if (res.success) this.cars = res.data;
        });
    }

    loadHouses() {
        this.houseService.getHousesByCustomer(this.customerId).subscribe(res => {
            if (res.success) this.houses = res.data;
        });
    }

    goAddCar() {
        this.router.navigate(['/mainpage/customer', this.customerId, 'add-car']);
    }

    goAddHouse() {
        this.router.navigate(['/mainpage/customer', this.customerId, 'add-house']);
    }

    goEditCar(carId: number) {
        this.router.navigate(['/mainpage/customer', this.customerId, 'edit-car', carId]);
    }

    goEditHouse(houseId: number) {
        this.router.navigate(['/mainpage/customer', this.customerId, 'edit-house', houseId]);
    }

    goBack() {
        this.router.navigate(['/mainpage/dashboard']);
    }

    openLightbox(relativePath: string, alt: string) {
        this.lightboxImage = 'http://localhost:5005/uploads/' + relativePath;
        this.lightboxAlt = alt;
    }

    closeLightbox() {
        this.lightboxImage = null;
    }
}
