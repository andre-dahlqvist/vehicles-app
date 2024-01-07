namespace VehiclesApi.Models.Dto;

public class VehicleDto
{
	public string Vin { get; set; }

	public string LicensePlateNumber { get; set; }

	public string ModelName { get; set; }

	public int BrandId { get; set; }

	public string BrandName { get; set; }

	public ICollection<int> EquipmentIds { get; set; }

	public ICollection<EquipmentDto> Equipment { get; set; }
}
