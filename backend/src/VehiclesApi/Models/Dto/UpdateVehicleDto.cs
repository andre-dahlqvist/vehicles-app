using System.ComponentModel.DataAnnotations;

namespace VehiclesApi.Models.Dto;

public class UpdateVehicleDto
{
	[Required]
	[MaxLength(20)]
	public string LicensePlateNumber { get; set; }

	[Required]
	[MaxLength(100)]
	public string ModelName { get; set; }

	[Required]
	public int BrandId { get; set; }

	public ICollection<int> EquipmentIds { get; set; }
}
