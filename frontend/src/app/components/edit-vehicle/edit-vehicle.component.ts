import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { takeUntil, catchError } from 'rxjs/operators';
import { of, Subject } from 'rxjs';
import { BrandsService } from '../../services/brands.service';
import { EquipmentService } from '../../services/equipment.service';
import { VehicleFormModel } from '../../models/vehicle.model';
import { Brand } from '../../models/brand.model';
import { Equipment } from '../../models/equipment.model';
import { VehiclesService } from '../../services/vehicles.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-vehicle',
  templateUrl: './edit-vehicle.component.html',
  styleUrls: ['./edit-vehicle.component.css'],
})
export class EditVehicleComponent implements OnInit, OnDestroy {
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
    private route: ActivatedRoute
  ) {
    this.fetchData();
  }

  ngOnInit() {}

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  fetchData() {
    this.brandsService
      .getAll()
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError((error) => {
          console.error('Error fetching brands:', error);
          return of([]);
        })
      )
      .subscribe((brands: Brand[]) => {
        this.brands = brands;
      });

    this.equipmentService
      .getAll()
      .pipe(
        takeUntil(this.unsubscribe$),
        catchError((error) => {
          console.error('Error fetching equipment:', error);
          return of([]);
        })
      )
      .subscribe((equipment: Equipment[]) => {
        this.equipment = equipment;
      });

    const vin = this.route.snapshot.paramMap.get('vin') || '';
    this.vehiclesService.get(vin).subscribe({
      next: (existingVehicle: VehicleFormModel) => {
        this.vehicle = existingVehicle;
      },
      error: (e) => console.error(e),
    });
  }

  save(): void {
    this.vehiclesService.update(this.vehicle.vin, this.vehicle).subscribe({
      next: (res) => {
        this.router.navigate(['/vehicles']);
      },
      error: (err) => {
        this.errorMessage = 'An error occurred while saving the vehicle. Please try again.';
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/vehicles']);
  }
}
