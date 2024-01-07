using Microsoft.AspNetCore.Mvc;
using VehiclesApi.Models;
using VehiclesApi.Services;

namespace VehiclesApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class EquipmentController : ControllerBase
	{
		private readonly IEquipmentService _equipmentService;

		public EquipmentController(IEquipmentService equipmentService)
		{
			_equipmentService = equipmentService;
		}

		/// <summary>
		/// Gets all equipment
		/// </summary>
		/// <returns>A list of equipment</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EquipmentDto))]
		public async Task<IEnumerable<EquipmentDto>> GetAllEquipment()
		{
			return await _equipmentService.GetEquipmentAsync();
		}
	}
}
