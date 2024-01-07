using VehiclesApi.Models;
using VehiclesApi.Services;

namespace VehiclesApi.UnitTests;

/// <summary>
/// Unit tests for <see cref="BrandsService"/>
/// </summary>
public class BrandsServiceTests : InMemoryDatabaseTestBase
{
	[Fact]
	public async void GetBrandsAsync_GetsAllBrands()
	{
		// Arrange
		var dbContext = GetDbContext();
		dbContext.Brands.Add(new Brand { Id = 1, Name = "Lada" });
		dbContext.Brands.Add(new Brand { Id = 2, Name = "Moskvich" });
		await dbContext.SaveChangesAsync();
		var service = new BrandsService(dbContext);

		// Act
		var brands = await service.GetBrandsAsync();

		// Assert
		Assert.Equal(2, brands.Count());
	}

	[Fact]
	public async void CheckIfExistsAsync_ReturnsTrue_ExistentBrand()
	{
		// Arrange
		var dbContext = GetDbContext();
		var existingBrand = new Brand { Id = 3, Name = "Sisu Auto" };
		dbContext.Brands.Add(existingBrand);
		await dbContext.SaveChangesAsync();

		var service = new BrandsService(dbContext);

		// Act
		var exists = await service.CheckIfExistsAsync(existingBrand.Id);

		// Assert
		Assert.True(exists);
	}

	[Fact]
	public async void CheckIfExistsAsync_ReturnsFalse_NonExistentBrand()
	{
		// Arrange
		var dbContext = GetDbContext();
		await dbContext.SaveChangesAsync();

		var service = new BrandsService(dbContext);

		// Act
		var exists = await service.CheckIfExistsAsync(42);

		// Assert
		Assert.False(exists);
	}
}