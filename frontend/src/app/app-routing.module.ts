import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehiclesListComponent } from './components/vehicles-list/vehicles-list.component';
import { AddVehicleComponent } from './components/add-vehicle/add-vehicle.component';
import { EditVehicleComponent } from './components/edit-vehicle/edit-vehicle.component';

const routes: Routes = [
  { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
  { path: 'vehicles', component: VehiclesListComponent },
  { path: 'edit/:vin', component: EditVehicleComponent },
  { path: 'add', component: AddVehicleComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
