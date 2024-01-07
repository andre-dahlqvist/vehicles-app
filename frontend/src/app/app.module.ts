import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddVehicleComponent } from './components/add-vehicle/add-vehicle.component';
import { EditVehicleComponent } from './components/edit-vehicle/edit-vehicle.component';
import { VehiclesListComponent } from './components/vehicles-list/vehicles-list.component';

@NgModule({
  declarations: [
    AppComponent,
    AddVehicleComponent,
    EditVehicleComponent,
    VehiclesListComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
