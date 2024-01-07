using Microsoft.AspNetCore.Mvc;
using Moq;
using VehiclesApi.Controllers;
using VehiclesApi.Exceptions;
using VehiclesApi.Models.Dto;
using VehiclesApi.Services;

namespace VehiclesApi.UnitTests;

/// <summary>
/// Unit tests for <see cref="VehiclesController"/>
/// </summary>
public class VehiclesControllerTests
{
	[Fact]
	public async void GetVehicleByVin_ReturnsNotFound_VehicleWithGivenVinNotFound()
	{
		// Arrange
		var vin = "123412341234";
		var mockVehiclesService = new Mock<IVehiclesService>();
		var mockBrandsService = new Mock<IBrandsService>();
		var mockEquipmentService = new Mock<IEquipmentService>();
		var controller = new VehiclesController(mockVehiclesService.Object, mockBrandsService.Object, mockEquipmentService.Object);
		mockVehiclesService.Setup(x => x.GetVehicleByVinAsync(It.IsAny<string>()))
			.ReturnsAsync((VehicleDto)null!);

		// Act
		var result = await controller.GetVehicleByVin(vin);

		// Assert
		Assert.IsType<NotFoundResult>(result.Result);
	}

	[Fact]
	public async void CreateVehicle_ReturnsBadRequest_BrandNotFound()
	{
		// Arrange
		const int brandId = 1;
		var mockVehiclesService = new Mock<IVehiclesService>();
		var mockBrandsService = new Mock<IBrandsService>();
		var mockEquipmentService = new Mock<IEquipmentService>();
		var controller = new VehiclesController(mockVehiclesService.Object, mockBrandsService.Object, mockEquipmentService.Object);
		mockBrandsService.Setup(x => x.CheckIfExistsAsync(brandId))
			.ReturnsAsync(false);
		var createVehicleDto = new CreateVehicleDto
		{
			BrandId = brandId,
		};

		// Act
		var result = await controller.CreateVehicle(createVehicleDto);

		// Assert
		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void CreateVehicle_ReturnsBadRequest_EquipmentNotFound()
	{
		// Arrange
		const int brandId = 1;
		var equipmentIds = new List<int> { 1, 2 };
		var mockVehiclesService = new Mock<IVehiclesService>();
		var mockBrandsService = new Mock<IBrandsService>();
		var mockEquipmentService = new Mock<IEquipmentService>();
		var controller = new VehiclesController(mockVehiclesService.Object, mockBrandsService.Object, mockEquipmentService.Object);
		mockBrandsService.Setup(x => x.CheckIfExistsAsync(brandId))
			.ReturnsAsync(true);
		mockEquipmentService.Setup(x => x.CheckIfAllExist(equipmentIds))
			.Returns(false);
		var createVehicleDto = new CreateVehicleDto
		{
			BrandId = brandId,
			EquipmentIds = equipmentIds
		};

		// Act
		var result = await controller.CreateVehicle(createVehicleDto);

		// Assert
		var badRequest = Assert.IsType<BadRequestObjectResult>(result);
		var errorMessage = Assert.IsType<string>(badRequest.Value);
		Assert.Equal("At least one of the provided equipment IDs does not exist", errorMessage);
	}

	[Fact]
	public async void CreateVehicle_ReturnsConflict_VehicleWithVinAlreadyExists()
	{
		// Arrange
		var existingVehicle = new VehicleDto { Vin = "12341234123443642" };
		var mockVehiclesService = new Mock<IVehiclesService>();
		var mockBrandsService = new Mock<IBrandsService>();
		var mockEquipmentService = new Mock<IEquipmentService>();
		var controller = new VehiclesController(mockVehiclesService.Object, mockBrandsService.Object, mockEquipmentService.Object);
		// Simulate that the brand exists so we get past that validation
		mockBrandsService.Setup(x => x.CheckIfExistsAsync(It.IsAny<int>()))
			.ReturnsAsync(true);
		// Same for equipment
		mockEquipmentService.Setup(x => x.CheckIfAllExist(It.IsAny<ICollection<int>>())).Returns(true);
		mockVehiclesService.Setup(x => x.GetVehicleByVinAsync(It.IsAny<string>()))
			.ReturnsAsync(existingVehicle);

		// Try to create a vehicle with an existing VIN
		var createVehicleDto = new CreateVehicleDto
		{
			Vin = existingVehicle.Vin
		};

		// Act
		var result = await controller.CreateVehicle(createVehicleDto);

		// Assert
		Assert.IsType<ConflictObjectResult>(result);
	}

	[Fact]
	public async void UpdateVehicle_ReturnsBadRequest_BrandNotFound()
	{
		// Arrange
		const int brandId = 1;
		const string vin = "83829283846637833";
		var mockVehiclesService = new Mock<IVehiclesService>();
		var mockBrandsService = new Mock<IBrandsService>();
		var mockEquipmentService = new Mock<IEquipmentService>();
		var controller = new VehiclesController(mockVehiclesService.Object, mockBrandsService.Object, mockEquipmentService.Object);
		mockBrandsService.Setup(x => x.CheckIfExistsAsync(brandId))
			.ReturnsAsync(false);
		var updateVehicleDto = new UpdateVehicleDto
		{
			BrandId = brandId,
		};

		// Act
		var result = await controller.UpdateVehicle(vin, updateVehicleDto);

		// Assert
		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void UpdateVehicle_ReturnsBadRequest_EquipmentNotFound()
	{
		// Arrange
		const int brandId = 1;
		const string vin = "123412341234";
		var equipmentIds = new List<int> { 1, 2 };
		var mockVehiclesService = new Mock<IVehiclesService>();
		var mockBrandsService = new Mock<IBrandsService>();
		var mockEquipmentService = new Mock<IEquipmentService>();
		var controller = new VehiclesController(mockVehiclesService.Object, mockBrandsService.Object, mockEquipmentService.Object);
		mockBrandsService.Setup(x => x.CheckIfExistsAsync(brandId))
			.ReturnsAsync(true);
		mockEquipmentService.Setup(x => x.CheckIfAllExist(equipmentIds))
			.Returns(false);
		var updateVehicleDto = new UpdateVehicleDto
		{
			BrandId = brandId,
			EquipmentIds = equipmentIds
		};

		// Act
		var result = await controller.UpdateVehicle(vin, updateVehicleDto);

		// Assert
		var badRequest = Assert.IsType<BadRequestObjectResult>(result);
		var errorMessage = Assert.IsType<string>(badRequest.Value);
		Assert.Equal("At least one of the provided equipment IDs does not exist", errorMessage);
	}

	[Fact]
	public async void DeleteVehicle_ReturnsNotFound_VehicleWithGivenVinNotFound()
	{
		// Arrange
		var vin = "123412341234";
		var mockVehiclesService = new Mock<IVehiclesService>();
		var mockBrandsService = new Mock<IBrandsService>();
		var mockEquipmentService = new Mock<IEquipmentService>();
		var controller = new VehiclesController(mockVehiclesService.Object, mockBrandsService.Object, mockEquipmentService.Object);
		mockVehiclesService.Setup(x => x.DeleteVehicleAsync(vin))
			.ThrowsAsync(new ObjectNotFoundException($"Vehicle with vin {vin} does not exist"));

		// Act
		var result = await controller.DeleteVehicle(vin);

		// Assert
		Assert.IsType<NotFoundResult>(result);
	}
}