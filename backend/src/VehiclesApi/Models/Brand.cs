using System.ComponentModel.DataAnnotations;

namespace VehiclesApi.Models
{
	public class Brand
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }
	}
}
