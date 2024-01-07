using Microsoft.EntityFrameworkCore;
using VehiclesApi.Data;
using VehiclesApi.Models;

namespace VehiclesApi.Services;

public interface IBrandsService
{
	Task<IEnumerable<BrandDto>> GetBrandsAsync();
	Task<bool> CheckIfExistsAsync(int brandId);
}

public class BrandsService(VehiclesDbContext dbContext) : IBrandsService
{
	private readonly VehiclesDbContext _dbContext = dbContext;

	public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
	{
		return await _dbContext.Brands.AsNoTracking()
			.Select(x => new BrandDto
			{
				Id = x.Id,
				Name = x.Name,
			})
			.ToListAsync();
	}

	public async Task<bool> CheckIfExistsAsync(int brandId)
	{
		return await _dbContext.Brands.AnyAsync(x => x.Id == brandId);
	}
}