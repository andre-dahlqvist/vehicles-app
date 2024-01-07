using System.ComponentModel.DataAnnotations;

namespace VehiclesApi.Models.Dto;

public class CreateVehicleDto
{
	public string Vin { get; set; }

	[MaxLength(20)]
	[Required]
	public string LicensePlateNumber { get; set; }

	[Required]
	[MaxLength(100)]
	public string ModelName { get; set; }

	[Required]
	public int BrandId { get; set; }

	public ICollection<int> EquipmentIds { get; set; }
}
