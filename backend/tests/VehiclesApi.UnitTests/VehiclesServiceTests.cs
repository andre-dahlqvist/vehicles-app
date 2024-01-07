using VehiclesApi.Exceptions;
using VehiclesApi.Models;
using VehiclesApi.Models.Dto;
using VehiclesApi.Services;

namespace VehiclesApi.UnitTests;

/// <summary>
/// Unit tests for <see cref="VehiclesService"/>
/// </summary>
public class VehiclesServiceTests : InMemoryDatabaseTestBase
{
	[Fact]
	public async void GetVehiclesAsync_GetsAllVehicles()
	{
		// Arrange
		var existingVehicle = new Vehicle
		{
			Vin = "1HGCM82633A000001",
			Brand = new Brand { Id = 1, Name = "Tesla" },
			ModelName = "Model X",
			LicensePlateNumber = "ABC123",
			VehicleEquipment = new List<VehicleEquipment>
			{
				new VehicleEquipment
				{
					Equipment = new Equipment
					{
						Id = 1,
						Name = "Leather seats"
					}
				}
			},
		};
		var dbContext = GetDbContext();
		dbContext.Vehicles.Add(existingVehicle);
		await dbContext.SaveChangesAsync();

		var service = new VehiclesService(dbContext);

		// Act
		var vehicles = await service.GetVehiclesAsync();

		// Assert
		var vehicle = Assert.Single(vehicles);
		Assert.Equal(existingVehicle.Vin, vehicle.Vin);
		Assert.Equal(existingVehicle.ModelName, vehicle.ModelName);
		Assert.Equal(existingVehicle.LicensePlateNumber, vehicle.LicensePlateNumber);
		Assert.Single(existingVehicle.VehicleEquipment);
	}

	[Fact]
	public async void GetVehicleByVinAsync_GetsVehicle()
	{
		// Arrange
		var existingVehicle = new Vehicle
		{
			Vin = "5YJSA1CN5DFP00000",
			Brand = new Brand { Id = 2, Name = "Toyota" },
			ModelName = "Corolla",
			LicensePlateNumber = "ZYC543",
			VehicleEquipment = new List<VehicleEquipment>
			{
				new VehicleEquipment
				{
					Equipment = new Equipment
					{
						Id = 2,
						Name = "Parking sensor"
					}
				}
			},
		};
		var dbContext = GetDbContext();
		dbContext.Vehicles.Add(existingVehicle);
		await dbContext.SaveChangesAsync();

		var service = new VehiclesService(dbContext);

		// Act
		var vehicleDto = await service.GetVehicleByVinAsync(existingVehicle.Vin);

		// Assert
		Assert.NotNull(vehicleDto);
		Assert.Equal(existingVehicle.Vin, vehicleDto.Vin);
		Assert.Equal(existingVehicle.ModelName, vehicleDto.ModelName);
		Assert.Equal(existingVehicle.LicensePlateNumber, vehicleDto.LicensePlateNumber);
		Assert.Equal(1, existingVehicle.VehicleEquipment.Count);
	}

	[Fact]
	public async void GetVehicleByVinAsync_ReturnsNull_VinNotFound()
	{
		var dbContext = GetDbContext();
		var service = new VehiclesService(dbContext);
		// No vehicle with this VIN is saved in the database
		var nonExistantVin = "11111111111111111";

		// Act
		var vehicleDto = await service.GetVehicleByVinAsync(nonExistantVin);

		// Assert
		Assert.Null(vehicleDto);
	}

	[Fact]
	public async void CreateVehicle_CreatesVehicle()
	{
		// Arrange
		var dbContext = GetDbContext();
		dbContext.Brands.Add(new Brand
		{
			Id = 1,
			Name = "Nissan"
		});
		await dbContext.SaveChangesAsync();
		var createVehicleDto = new CreateVehicleDto
		{
			Vin = "3VWCA21C9XM000011",
			BrandId = 1,
			ModelName = "Qashqai",
			LicensePlateNumber = "BCD233",
			EquipmentIds = new[] { 1, 2 }
		};

		var service = new VehiclesService(dbContext);

		// Act
		var createdVehicle = await service.CreateVehicleAsync(createVehicleDto);

		// Assert
		Assert.NotNull(createdVehicle);
	}

	[Fact]
	public async void CreateVehicle_ThrowsException_BrandNotFound()
	{
		// Arrange
		var createVehicleDto = new CreateVehicleDto
		{
			BrandId = 99, // No brand with this id exists in the DB
		};
		var dbContext = GetDbContext();
		var service = new VehiclesService(dbContext);

		// Act
		var exception = await Record.ExceptionAsync(async () => await service.CreateVehicleAsync(createVehicleDto));

		// Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidOperationException>(exception);
	}

	[Fact]
	public async void UpdateVehicle_UpdatesExpectedProperties()
	{
		// Arrange
		var dbContext = GetDbContext();
		dbContext.Brands.Add(new Brand
		{
			Id = 1,
			Name = "Hyundai"
		});
		var existingVehicle = new Vehicle
		{
			Vin = "SAJDA42C52NA000015",
			Brand = new Brand { Id = 2, Name = "Audi" },
			ModelName = "A6",
			LicensePlateNumber = "JTE892",
			VehicleEquipment = new List<VehicleEquipment>()
		};
		dbContext.Vehicles.Add(existingVehicle);
		await dbContext.SaveChangesAsync();
		var updateVehicleDto = new UpdateVehicleDto
		{
			BrandId = 1,
			ModelName = "Ioniq 5",
			LicensePlateNumber = "GTW234",
			EquipmentIds = new[] { 2 }
		};

		var service = new VehiclesService(dbContext);

		// Act
		await service.UpdateVehicleAsync(existingVehicle.Vin, updateVehicleDto);

		// Assert
		var dbVehicle = await dbContext.Vehicles.FindAsync(existingVehicle.Vin);
		Assert.NotNull(dbVehicle);
		Assert.Equal(updateVehicleDto.ModelName, dbVehicle.ModelName);
		Assert.Equal(updateVehicleDto.LicensePlateNumber, dbVehicle.LicensePlateNumber);
		Assert.Equal(updateVehicleDto.BrandId, dbVehicle.BrandId);
		Assert.Single(dbVehicle.VehicleEquipment);
	}

	[Fact]
	public async void UpdateVehicle_ThrowsException_VehicleWithGivenVinNotFound()
	{
		// Arrange
		var vin = "113413412341234"; // No vehicle with this VIN exists
		var updateVehicleDto = new UpdateVehicleDto();
		var dbContext = GetDbContext();
		var service = new VehiclesService(dbContext);

		// Act
		var exception = await Record.ExceptionAsync(async () => await service.UpdateVehicleAsync(vin, updateVehicleDto));

		// Assert
		Assert.NotNull(exception);
		Assert.IsType<ObjectNotFoundException>(exception);
	}

	[Fact]
	public async void UpdateVehicle_ThrowsException_BrandNotFound()
	{
		// Arrange
		var dbContext = GetDbContext();
		var existingVehicle = new Vehicle
		{
			Vin = "1FTYR10C1XPA000016",
			Brand = new Brand { Id = 2, Name = "Audi" },
			ModelName = "A6",
			LicensePlateNumber = "JTE892",
			VehicleEquipment = new List<VehicleEquipment>()
		};
		dbContext.Vehicles.Add(existingVehicle);
		await dbContext.SaveChangesAsync();
		var updateVehicleDto = new UpdateVehicleDto
		{
			BrandId = 99, // No brand with this id exists in the DB
		};
		var service = new VehiclesService(dbContext);

		// Act
		var exception = await Record.ExceptionAsync(async () => await service.UpdateVehicleAsync(existingVehicle.Vin, updateVehicleDto));

		// Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidOperationException>(exception);
	}

	[Fact]
	public async Task DeleteVehicle_DeletesVehicle()
	{
		// Arrange
		var dbContext = GetDbContext();
		var existingVehicle = new Vehicle
		{
			Vin = "1GNSCBE06ER000012",
			Brand = new Brand { Id = 2, Name = "Audi" },
			ModelName = "A6",
			LicensePlateNumber = "ERR223",
			VehicleEquipment = new List<VehicleEquipment>()
		};
		dbContext.Vehicles.Add(existingVehicle);
		await dbContext.SaveChangesAsync();
		var service = new VehiclesService(dbContext);

		// Act
		await service.DeleteVehicleAsync(existingVehicle.Vin);

		// Assert
		var dbVehicle = await dbContext.Vehicles.FindAsync(existingVehicle.Vin);
		Assert.Null(dbVehicle);
	}

	[Fact]
	public async void DeleteVehicle_ThrowsException_VehicleWithGivenVinNotFound()
	{
		// Arrange
		var vin = "5LMFU28535LJ000009"; // No vehicle with this VIN exists
		var dbContext = GetDbContext();
		var service = new VehiclesService(dbContext);

		// Act
		var exception = await Record.ExceptionAsync(async () => await service.DeleteVehicleAsync(vin));

		// Assert
		Assert.NotNull(exception);
		Assert.IsType<ObjectNotFoundException>(exception);
	}
}