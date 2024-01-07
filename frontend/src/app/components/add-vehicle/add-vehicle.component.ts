import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { takeUntil, catchError } from 'rxjs/operators';
import { forkJoin, of, Subject } from 'rxjs';
import { BrandsService } from '../../services/brands.service';
import { EquipmentService } from '../../services/equipment.service';
import { VehicleFormModel } from '../../models/vehicle.model';
import { Brand } from '../../models/brand.model';
import { Equipment } from '../../models/equipment.model';
import { VehiclesService } from '../../services/vehicles.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-vehicle',
  templateUrl: './add-vehicle.component.html',
  styleUrls: ['./add-vehicle.component.css'],
})
export class AddVehicleComponent implements OnInit, OnDestroy {
  @Input() vehicle: VehicleFormModel = {
    vin: '',
    modelName: '',
    brandId: null,
    licensePlateNumber: '',
    equipmentIds: [],
  };
  brands: Brand[] = [];
  equipment: Equipment[] = [];
  errorMessage: string = '';
  private unsubscribe$ = new Subject<void>();

  constructor(
    private brandsService: BrandsService,
    private equipmentService: EquipmentService,
    private vehiclesService: VehiclesService,
    private router: Router,
  ) {
    this.fetchData();
  }

  ngOnInit() {}

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  fetchData() {
    const brands$ = this.brandsService.getAll();
    const equipment$ = this.equipmentService.getAll();
  
    forkJoin([brands$, equipment$])
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError((error) => {
          console.error('Error fetching brands or equipment:', error);
          return of([]);
        })
      )
      .subscribe(([brands, equipment]) => {
        this.brands = brands;
        this.equipment = equipment;
      });
  }

  save(): void {
    this.vehiclesService.create(this.vehicle).subscribe({
      next: (res) => {
        this.router.navigate(['/vehicles']);
      },
      error: (err) => {
        if (err.status === 409) {
          // Conflict error; vehicle with same VIN already exists
          this.errorMessage = 'Vehicle with this VIN already exists. Please use a different VIN.';
        } else {
          // Handle other error scenarios
          this.errorMessage = 'An error occurred while saving the vehicle. Please try again.';
        }
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/vehicles']);
  }
}
