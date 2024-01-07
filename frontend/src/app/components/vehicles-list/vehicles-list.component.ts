import { Component, OnInit } from '@angular/core';
import { Vehicle } from '../../models/vehicle.model';
import { VehiclesService } from '../../services/vehicles.service';
import { switchMap } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vehicles-list',
  templateUrl: './vehicles-list.component.html',
  styleUrls: ['./vehicles-list.component.css'],
})
export class VehiclesListComponent implements OnInit {
  vehicles?: Vehicle[];
  errorMessage: string = '';

  constructor(
    private vehiclesService: VehiclesService,
    private router: Router) {}

  ngOnInit(): void {
    this.getVehicles();
  }

  getVehicles(): void {
    this.vehiclesService.getAll().subscribe({
      next: (data) => {
        this.vehicles = data;
      },
      error: (error) => {
        this.errorMessage = `Failed to fetch vehicles: ${error.statusText}`;
      }
    });
  }

  confirmDelete(vehicle: any) {
    const isConfirmed = window.confirm(`Are you sure you want to delete ${vehicle.vin}?`);

    if (isConfirmed) {
      this.vehiclesService.delete(vehicle.vin)
        .pipe(
          switchMap(() => this.vehiclesService.getAll())
        )
        .subscribe({
          next: (updatedVehicles) => {
            this.vehicles = updatedVehicles;
          },
          error: (error) => {
            this.errorMessage = `Error deleting vehicle: ${error}`;
          }
        });
    }
  }

  edit(vehicle: any) {
    this.router.navigate(['edit', vehicle.vin]);
  }

  addNewVehicle() {
    this.router.navigate(['add']);
  }
}
