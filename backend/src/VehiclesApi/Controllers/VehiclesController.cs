using Microsoft.AspNetCore.Mvc;
using VehiclesApi.Exceptions;
using VehiclesApi.Models.Dto;
using VehiclesApi.Services;

namespace VehiclesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ControllerBase
{
	private readonly IVehiclesService _vehiclesService;
	private readonly IBrandsService _brandsService;
	private readonly IEquipmentService _equipmentService;

	public VehiclesController(IVehiclesService vehiclesService, IBrandsService brandsService, IEquipmentService equipmentService)
	{
		_vehiclesService = vehiclesService;
		_brandsService = brandsService;
		_equipmentService = equipmentService;
	}

	/// <summary>
	/// Gets all vehicles
	/// </summary>
	/// <returns>A list of vehicles</returns>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDto))]
	public async Task<IEnumerable<VehicleDto>> GetVehiclesAsync()
	{
		return await _vehiclesService.GetVehiclesAsync();
	}

	/// <summary>
	/// Gets a vehicle by VIN. NotFound is returned
	/// if a vehicle with the given VIN does not exist.
	/// </summary>
	/// <param name="vin">The VIN (Vehicle Identification Number) of the vehicle to get</param>
	[HttpGet("{vin}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDto))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<VehicleDto>> GetVehicleByVin(string vin)
	{
		var vehicle = await _vehiclesService.GetVehicleByVinAsync(vin);

		if (vehicle == null)
		{
			return NotFound();
		}

		return Ok(vehicle);
	}

	/// <summary>
	/// Creates a new vehicle.
	/// </summary>
	/// <param name="createVehicleDto">The vehicle to create</param>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VehicleDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(VehicleDto))]
	[ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(VehicleDto))]
	public async Task<ActionResult> CreateVehicle(CreateVehicleDto createVehicleDto)
	{
		// Validate that the brand exists
		var brandExists = await _brandsService.CheckIfExistsAsync(createVehicleDto.BrandId);
		if (!brandExists)
		{
			return BadRequest($"Brand with id {createVehicleDto.BrandId} does not exist");
		}

		// Validate that all equipment IDs exist
		var allEquipmentIdsExist = _equipmentService.CheckIfAllExist(createVehicleDto.EquipmentIds);
		if (!allEquipmentIdsExist)
		{
			return BadRequest("At least one of the provided equipment IDs does not exist");
		}

		// Make sure that the VIN is unique
		var existingVehicle = await _vehiclesService.GetVehicleByVinAsync(createVehicleDto.Vin);
		if (existingVehicle != null)
		{
			return Conflict($"Vehicle with VIN {createVehicleDto.Vin} already exists");
		}

		var savedVehicle = await _vehiclesService.CreateVehicleAsync(createVehicleDto);
		if (savedVehicle == null)
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return CreatedAtAction(nameof(GetVehicleByVin), new { vin = savedVehicle.Vin }, null);
	}

	/// <summary>
	/// Creates a new vehicle.
	/// </summary>
	/// <param name="vin">The VIN of the vehicle to update</param>
	/// <param name="updateVehicleDto">The updated vehicle parameters</param>
	[HttpPut("{vin}")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDto))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(VehicleDto))]
	public async Task<ActionResult> UpdateVehicle(string vin, UpdateVehicleDto updateVehicleDto)
	{
		// Validate that the brand exists
		var brandExists = await _brandsService.CheckIfExistsAsync(updateVehicleDto.BrandId);
		if (!brandExists)
		{
			return BadRequest($"Brand with id {updateVehicleDto.BrandId} does not exist");
		}

		// Validate that all equipment IDs exist
		var allEquipmentIdsExist = _equipmentService.CheckIfAllExist(updateVehicleDto.EquipmentIds);
		if (!allEquipmentIdsExist)
		{
			return BadRequest("At least one of the provided equipment IDs does not exist");
		}

		try
		{
			await _vehiclesService.UpdateVehicleAsync(vin, updateVehicleDto);
		}
		catch (ObjectNotFoundException)
		{
			return NotFound();
		}

		var dbVehicle = await _vehiclesService.GetVehicleByVinAsync(vin);

		return Ok(dbVehicle);
	}

	/// <summary>
	/// Deletes the vehicle with the given VIN.
	/// </summary>
	/// <param name="vin">The VIN of the vehicle to delete</param>
	[HttpDelete("{vin}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteVehicle(string vin)
	{
		try
		{
			await _vehiclesService.DeleteVehicleAsync(vin);
		}
		catch (ObjectNotFoundException)
		{
			return NotFound();
		}

		return NoContent();
	}
}