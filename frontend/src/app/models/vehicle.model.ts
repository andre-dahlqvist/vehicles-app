import { Equipment } from "./equipment.model";

export class Vehicle {
  vin?: string;
  licensePlateNumber?: string;
  modelName?: string;
  brandName?: string;
  equipment?: Array<Equipment> = [];
}

export class VehicleFormModel {
  vin?: string;
  licensePlateNumber?: string;
  modelName?: string;
  brandId?: number | null;
  equipmentIds?: Array<number> = [];
}

export class EditVehicleModel {
  vin?: string;
  licensePlateNumber?: string;
  modelName?: string;
  brandId?: number | null;
  equipment?: Array<Equipment> = [];
}