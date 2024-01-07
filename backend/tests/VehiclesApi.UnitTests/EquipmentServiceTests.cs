using VehiclesApi.Models;
using VehiclesApi.Services;

namespace VehiclesApi.UnitTests;

/// <summary>
/// Unit tests for <see cref="EquipmentService"/>
/// </summary>
public class EquipmentServiceTests : InMemoryDatabaseTestBase
{
	[Fact]
	public async void GetEquipmentAsync_GetsAllEquipment()
	{
		// Arrange
		var dbContext = GetDbContext();
		dbContext.Equipment.Add(new Equipment { Id = 1, Name = "Parking sensor" });
		dbContext.Equipment.Add(new Equipment { Id = 2, Name = "Leather seats" });
		await dbContext.SaveChangesAsync();
		var service = new EquipmentService(dbContext);

		// Act
		var equipment = await service.GetEquipmentAsync();

		// Assert
		Assert.Equal(2, equipment.Count());
	}

	[Fact]
	public async void CheckIfExistsAsync_ReturnsTrue_GivenEquipmentIdsExist()
	{
		// Arrange
		var dbContext = GetDbContext();
		dbContext.Equipment.AddRange(new List<Equipment>
		{
			new Equipment { Id = 1, Name =  "Rear view camera" },
			new Equipment { Id = 2, Name =  "Rocket Launcher" },
		});
		await dbContext.SaveChangesAsync();

		var service = new EquipmentService(dbContext);

		// Act
		var exists = service.CheckIfAllExist(new List<int> { 1, 2 });

		// Assert
		Assert.True(exists);
	}

	[Fact]
	public async void CheckIfExistsAsync_ReturnsFalse_NotAllOfTheGivenEquipmentIdsExist()
	{
		// Arrange
		var dbContext = GetDbContext();
		dbContext.Equipment.AddRange(new List<Equipment>
		{
			new Equipment { Id = 1, Name =  "Rear view camera" }
		});
		await dbContext.SaveChangesAsync();

		var service = new EquipmentService(dbContext);

		// Act
		// No equipment with id = 3 exists
		var exists = service.CheckIfAllExist(new List<int> { 1, 3 });

		// Assert
		Assert.False(exists);
	}
}