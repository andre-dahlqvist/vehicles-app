using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VehiclesApi.Data;
using VehiclesApi.Exceptions;
using VehiclesApi.Models;
using VehiclesApi.Models.Dto;

namespace VehiclesApi.Services;

public interface IVehiclesService
{
	Task<IEnumerable<VehicleDto>> GetVehiclesAsync();
	Task<VehicleDto> GetVehicleByVinAsync(string vin);
	Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto createVehicleDto);
	Task UpdateVehicleAsync(string vin, UpdateVehicleDto updateVehicleDto);
	Task DeleteVehicleAsync(string vin);
}

public class VehiclesService(VehiclesDbContext dbContext) : IVehiclesService
{
	private readonly VehiclesDbContext _dbContext = dbContext;

	public async Task<IEnumerable<VehicleDto>> GetVehiclesAsync()
	{
		return await _dbContext.Vehicles.AsNoTracking()
			.Include(x => x.Brand)
			.Include(x => x.VehicleEquipment)
				.ThenInclude(x => x.Equipment)
			.Select(MapToVehicleDto)
			.ToListAsync();
	}

	public async Task<VehicleDto> GetVehicleByVinAsync(string vin)
	{
		return await _dbContext.Vehicles.AsNoTracking()
			.Include(v => v.Brand)
			.Include(v => v.VehicleEquipment)
				.ThenInclude(ve => ve.Equipment)
			.Select(MapToVehicleDto)
			.FirstOrDefaultAsync(v => v.Vin == vin);
	}

	public async Task<VehicleDto> CreateVehicleAsync(CreateVehicleDto createVehicleDto)
	{
		var brand = await _dbContext.Brands.FindAsync(createVehicleDto.BrandId);
		if (brand == null)
		{
			throw new InvalidOperationException($"Brand with id {createVehicleDto.BrandId} does not exist");
		}

		var vehicle = new Vehicle
		{
			Vin = createVehicleDto.Vin,
			Brand = brand,
			ModelName = createVehicleDto.ModelName,
			LicensePlateNumber = createVehicleDto.LicensePlateNumber,
			VehicleEquipment = createVehicleDto.EquipmentIds
				.Select(equipmentId => new VehicleEquipment { EquipmentId = equipmentId })
				.ToList()
		};

		_dbContext.Vehicles.Add(vehicle);
		await _dbContext.SaveChangesAsync();

		return await GetVehicleByVinAsync(createVehicleDto.Vin);
	}

	public async Task UpdateVehicleAsync(string vin, UpdateVehicleDto updateVehicleDto)
	{
		var vehicle = await _dbContext.Vehicles
			.Include(x => x.VehicleEquipment)
			.FirstOrDefaultAsync(x => x.Vin == vin);

		if (vehicle == null)
		{
			throw new ObjectNotFoundException($"Vehicle with vin {vin} does not exist");
		}

		var brand = await _dbContext.Brands.FindAsync(updateVehicleDto.BrandId);
		if (brand == null)
		{
			throw new InvalidOperationException($"Brand with id {updateVehicleDto.BrandId} does not exist");
		}

		vehicle.LicensePlateNumber = updateVehicleDto.LicensePlateNumber;
		vehicle.ModelName = updateVehicleDto.ModelName;
		vehicle.BrandId = updateVehicleDto.BrandId;
		vehicle.VehicleEquipment = updateVehicleDto.EquipmentIds
			.Select(equipmentId => new VehicleEquipment { EquipmentId = equipmentId })
			.ToList();

		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteVehicleAsync(string vin)
	{
		var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(x => x.Vin == vin);
		if (vehicle == null)
		{
			throw new ObjectNotFoundException($"Vehicle with vin {vin} does not exist");
		}

		_dbContext.Vehicles.Remove(vehicle);
		await _dbContext.SaveChangesAsync();
	}

	private static Expression<Func<Vehicle, VehicleDto>> MapToVehicleDto =>
		v => new VehicleDto
		{
			Vin = v.Vin,
			BrandId = v.Brand.Id,
			BrandName = v.Brand.Name,
			LicensePlateNumber = v.LicensePlateNumber,
			ModelName = v.ModelName,
			EquipmentIds = v.VehicleEquipment.Select(ve => ve.EquipmentId).ToList(),
			Equipment = v.VehicleEquipment.Select(ve => new EquipmentDto { Id = ve.EquipmentId, Name = ve.Equipment.Name  }).ToList()
		};
}