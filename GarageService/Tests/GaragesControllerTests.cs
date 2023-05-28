using AutoMapper;
using Carservice.Controllers;
using GarageService.Application.LogicContracts;
using GarageService.Dtos;
using GarageService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GarageService.Tests;

public class GaragesControllerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IGarageLogic> _logicMock;
    private readonly GaragesController _controller;

    public GaragesControllerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _logicMock = new Mock<IGarageLogic>();
        _controller = new GaragesController(_mapperMock.Object, _logicMock.Object);
    }

    // ----------------------------- GET ALL GARAGE --------------------------------------------


    [Fact]
    public async Task GetAllGaragesAsync_ReturnsOkResult_WhenGaragesExist()
    {
        var garageQueryDto = new GarageQueryDto();
        var garageQuery = new GarageQuery();
        var garages = new List<Garage>
        {
            new Garage { Id = 1, Name = "Garage 1", Capacity = 10, Cars = new List<Car>() },
            new Garage { Id = 2, Name = "Garage 2", Capacity = 5, Cars = new List<Car>() }
        };
        var garageReadDtos = new List<GarageReadDto>
        {
            new GarageReadDto { Id = 1, Name = "Garage 1", AvailableSlots = 10 },
            new GarageReadDto { Id = 2, Name = "Garage 2", AvailableSlots = 5 }
        };

        _mapperMock.Setup(m => m.Map<GarageQuery>(garageQueryDto)).Returns(garageQuery);
        _logicMock.Setup(l => l.GetAllGaragesAsync(garageQuery)).ReturnsAsync(garages);
        _mapperMock.Setup(m => m.Map<IEnumerable<GarageReadDto>>(garages)).Returns(garageReadDtos);

        var result = await _controller.GetAllGaragesAsync(garageQueryDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IEnumerable<GarageReadDto>>(okResult.Value);
        Assert.Equal(garageReadDtos, model);
    }



    [Fact]
    public async Task GetAllGaragesAsync_ReturnsNotFoundResult_WhenGaragesDontExist()
    {
        // Arrange
        var garageQueryDto = new GarageQueryDto();
        var garageQuery = new GarageQuery();
        List<Garage> garages = null;
        List<GarageReadDto> garageReadDtos = null;


        _mapperMock.Setup(m => m.Map<GarageQuery>(garageQueryDto)).Returns(garageQuery);
        _logicMock.Setup(l => l.GetAllGaragesAsync(garageQuery)).ReturnsAsync(garages);
        _mapperMock.Setup(m => m.Map<IEnumerable<GarageReadDto>>(garages)).Returns(garageReadDtos);


        // Act
        var result = await _controller.GetAllGaragesAsync(garageQueryDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }


    // ----------------------------- CREATE GARAGE --------------------------------------------


    [Fact]
    public async Task CreateGarageAsync_ReturnsOkResult_WhenGaragesCreated()
    {
        // Arrange
        var garageCreateDto = new GarageCreateDto
        {
            Name = "TopGarage",
            Capacity = 15
        };
        var garage = new Garage
        {
            Id = 1,
            Name = "TopGarage",
            Capacity = 15
        };
        var createdGarage = new Garage
        {
            Id = 1,
            Name = "TopGarage",
            Capacity = 15
        };
        var createdDto = new GarageReadDto
        {
            Id = 1,
            Name = "TopGarage",
            Capacity = 15
        };

        _mapperMock.Setup(m => m.Map<Garage>(garageCreateDto)).Returns(garage);
        _logicMock.Setup(l => l.CreateGarageAsync(garage)).ReturnsAsync(createdGarage);
        _mapperMock.Setup(m => m.Map<GarageReadDto>(createdGarage)).Returns(createdDto);

        // Act
        var result = await _controller.CreateGarageAsync(garageCreateDto);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var model = Assert.IsType<GarageReadDto>(createdResult.Value);
        Console.WriteLine(model);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(createdDto, model);
    }

    // ----------------------------- GET GARAGE BY ID --------------------------------------------

    [Fact]
    public async Task GetGarageById_WhenGarageExists_ReturnsOkResult()
    {
        // Arrange
        int garageId = 1;
        var garage = new Garage { Id = garageId, Name = "Test Garage" };
        var garageReadDto = new GarageReadDto { Id = garageId, Name = "Test Garage" };


        _logicMock.Setup(logic => logic.GetGarageAsync(garageId)).ReturnsAsync(garage);
        _mapperMock.Setup(mapper => mapper.Map<GarageReadDto>(garage)).Returns(garageReadDto);

        // Act
        var result = await _controller.GetGarageById(garageId);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);

        var okResult = result.Result as OkObjectResult;
        Assert.Equal(garageReadDto, okResult?.Value);
    }


    [Fact]
    public async Task GetGarageById_WhenGarageDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        int garageId = 1;
        Garage? garage = null;
        GarageReadDto? garageReadDto = null;

        _logicMock.Setup(logic => logic.GetGarageAsync(garageId)).ReturnsAsync(garage);
        _mapperMock.Setup(mapper => mapper.Map<GarageReadDto>(garage)).Returns(garageReadDto);

        // Act
        var result = await _controller.GetGarageById(garageId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    // ----------------------------- DELETE GARAGE BY ID --------------------------------------------

    [Fact]
    public async Task DeleteGarageAsync_WhenGarageExists_ReturnsNoContentResult()
    {
        // Arrange
        int garageId = 1;

        _logicMock.Setup(logic => logic.DeleteGarageAsync(garageId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteGarageAsync(garageId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }


}







