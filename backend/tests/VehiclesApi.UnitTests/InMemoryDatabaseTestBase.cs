using Microsoft.EntityFrameworkCore;
using VehiclesApi.Data;

namespace VehiclesApi.UnitTests;

/// <summary>
/// Base class to use in tests to make sure that tests are isolated.
/// </summary>
public abstract class InMemoryDatabaseTestBase
{
	private readonly DbContextOptions<VehiclesDbContext> _options = new DbContextOptionsBuilder<VehiclesDbContext>()
		.UseInMemoryDatabase(databaseName: $"VehiclesUnitTests_{Guid.NewGuid()}")
		.Options;

	public VehiclesDbContext GetDbContext()
	{
		return new VehiclesDbContext(_options);
	}
}
