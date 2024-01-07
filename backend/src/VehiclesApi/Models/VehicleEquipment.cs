namespace VehiclesApi.Models;

public class VehicleEquipment
{
	public string Vin { get; set; } = string.Empty;
	public Vehicle Vehicle { get; set; }

	public int EquipmentId { get; set; }
	public Equipment Equipment { get; set; }
}
