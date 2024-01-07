using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehiclesApi.Models;

[Index(nameof(LicensePlateNumber), IsUnique = true)]
public class Vehicle
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	[MaxLength(17)]
	public string Vin { get; set; }

	[MaxLength(20)]
	[Required]
	public string LicensePlateNumber { get; set; }

	[Required]
	[MaxLength(100)]
	public string ModelName { get; set; }

	[ForeignKey("Brand")]
	public int BrandId { get; set; }

	[Required]
	public Brand Brand { get; set; }

	public ICollection<VehicleEquipment> VehicleEquipment { get; set; }
}
