import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EditVehicleModel, Vehicle } from '../models/vehicle.model';
import { environment } from '../../environments/environment';

const baseUrl = environment.apiBaseUrl + 'vehicles';

@Injectable({
  providedIn: 'root',
})
export class VehiclesService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(baseUrl);
  }

  get(id: string): Observable<EditVehicleModel> {
    return this.http.get<Vehicle>(`${baseUrl}/${id}`);
  }

  create(data: any): Observable<any> {
    return this.http.post(baseUrl, data);
  }

  update(id: any, data: any): Observable<any> {
    return this.http.put(`${baseUrl}/${id}`, data);
  }

  delete(vin: any): Observable<any> {
    console.log('delete called with id = ' + vin);
    return this.http.delete(`${baseUrl}/${vin}`);
  }

  deleteAll(): Observable<any> {
    return this.http.delete(baseUrl);
  }
}
