using Microsoft.EntityFrameworkCore;
using VehiclesApi.Data;
using VehiclesApi.Models;

namespace VehiclesApi.Services;

public interface IEquipmentService
{
	Task<IEnumerable<EquipmentDto>> GetEquipmentAsync();
	bool CheckIfAllExist(ICollection<int> equipmentIds);
}

public class EquipmentService(VehiclesDbContext dbContext) : IEquipmentService
{
	private readonly VehiclesDbContext _dbContext = dbContext;

	public async Task<IEnumerable<EquipmentDto>> GetEquipmentAsync()
	{
		return await _dbContext.Equipment.AsNoTracking()
			.Select(x => new EquipmentDto
			{
				Id = x.Id,
				Name = x.Name,
			})
			.ToListAsync();
	}

	public bool CheckIfAllExist(ICollection<int> equipmentIds)
	{
		return equipmentIds?.All(id => _dbContext.Equipment.Any(e => e.Id == id)) == true;
	}
}