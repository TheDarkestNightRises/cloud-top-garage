using Xunit;
using Moq;
using AutoMapper;
using Application.LogicContracts;
using Carservice.Controllers;
using CarService.Dtos;
using CarService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tests;

public class CarsControllerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICarLogic> _logicMock;
    private readonly CarsController _controller;

    public CarsControllerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _logicMock = new Mock<ICarLogic>();
        _controller = new CarsController(_mapperMock.Object, _logicMock.Object);
    }

    [Fact]
    public async Task GetAllCarsAsync_ReturnsOkObjectResult_WhenCarsExist()
    {
        // Arrange
        var carQueryDto = new CarQueryDto();
        var carQuery = new CarQuery();

        //car1
        var garageDto = new GarageDto{ Id = 1 };
        var engine = new Engine{Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400};

        var garage = new Garage{ Id = 1 };

        var car1 = new Car {Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engine, Garage = garage };
        
        var engineReadDto = new EngineReadDto{ Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

        var carReadDto1 = new CarReadDto { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engineReadDto, Garage = garageDto };
        
        //car2
        var car2 = new Car { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engine, Garage = garage };
        var carReadDto2 = new CarReadDto { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engineReadDto, Garage = garageDto };


        var cars = new List<Car> { car1, car2 };
        var carReadDtos = new List<CarReadDto> { carReadDto1, carReadDto2 };

        //_mapperMock.Setup(m => m.Map<Garage>(garageDto)).Returns(garage);

        //_mapperMock.Setup(m => m.Map<CarReadDto>(car)).Returns(carReadDto);
        _mapperMock.Setup(m => m.Map<EngineReadDto>(engine)).Returns(engineReadDto);
        _mapperMock.Setup(m => m.Map<GarageDto>(garage)).Returns(garageDto);
        
        _mapperMock.Setup(x => x.Map<CarQuery>(carQueryDto)).Returns(carQuery);
        _logicMock.Setup(x => x.GetAllCarsAsync(carQuery)).ReturnsAsync(cars);
        _mapperMock.Setup(x => x.Map<IEnumerable<CarReadDto>>(cars)).Returns(carReadDtos);

        // Act
        var result = await _controller.GetAllCarsAsync(carQueryDto);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
        var okObjectResult = result.Result as OkObjectResult;
        Assert.Equal(carReadDtos, okObjectResult.Value);
    }

    [Fact]
    public async Task GetAllCarsAsync_ReturnsNotFoundResult_WhenNoCarsExist()
    {
        // Arrange
        var carQueryDto = new CarQueryDto();
        var carQuery = new CarQuery();
        var cars = new List<Car>();
        _mapperMock.Setup(x => x.Map<CarQuery>(carQueryDto)).Returns(carQuery);
        _logicMock.Setup(x => x.GetAllCarsAsync(carQuery)).ReturnsAsync(cars);

        // Act
        var result = await _controller.GetAllCarsAsync(carQueryDto);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    [Fact]
    public async Task UpdateCar_ReturnsUpdatedCar_WhenCarExists()
    {
        // Arrange
        var garageDto = new GarageDto{ Id = 1 };

        var carUpdateDto = new CarUpdateDto{ Id = 1, Garage = garageDto };
        var garage = new Garage{ Id = 1 };
        var car = new Car{ Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Garage = garage };
        _mapperMock.Setup(x => x.Map<Car>(carUpdateDto)).Returns(car);
        _mapperMock.Setup(m => m.Map<Garage>(garageDto)).Returns(garage);
        _logicMock.Setup(x => x.UpdateCarAsync(car)).ReturnsAsync(car);

        // Act
        var result = await _controller.UpdateCarAsync(carUpdateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task UpdateCar_ReturnsStatusCode500_WhenExceptionThrown()
    {
        // Arrange
        var carUpdateDto = new CarUpdateDto();
        var car = new Car();
        _mapperMock.Setup(x => x.Map<Car>(carUpdateDto)).Returns(car);
        _logicMock.Setup(x => x.UpdateCarAsync(car)).Throws(new Exception());

        // Act
        var result = await _controller.UpdateCarAsync(carUpdateDto);

        // // Assert
        // Assert.IsType<NoContentResult>(result);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(500, objectResult.StatusCode);
    }

    [Fact]
    public async Task UpdateCar_ReturnsNotFoundResult_WhenCarDoesNotExist()
    {
        // Act
        var carUpdateDto = new CarUpdateDto();
        var car = new Car { Id = 1, Manufacturer = "Toyota", Model = "Camry", Year = 2021 };
        _mapperMock.Setup(x => x.Map<Car>(carUpdateDto)).Returns(car);
        _logicMock.Setup(x => x.UpdateCarAsync(car)).Throws(new ArgumentException($"There is no car with the id: {carUpdateDto.Id}"));

        // Act
        var result = await _controller.UpdateCarAsync(carUpdateDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
