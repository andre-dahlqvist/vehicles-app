using Microsoft.AspNetCore.Mvc;
using VehiclesApi.Models;
using VehiclesApi.Services;

namespace VehiclesApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class BrandsController : ControllerBase
	{
		private readonly IBrandsService _brandsService;

		public BrandsController(IBrandsService brandsService)
		{
			_brandsService = brandsService;
		}

		/// <summary>
		/// Gets all brands
		/// </summary>
		/// <returns>A list of brands</returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
		public async Task<IEnumerable<BrandDto>> GetAllBrands()
		{
			return await _brandsService.GetBrandsAsync();
		}
	}
}
