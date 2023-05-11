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

    [Fact]
    public async Task CreateCar_ValidCar_ReturnsCreatedWithCorrectObject()
    {
        // Arrange
        var engineCreateDto = new EngineCreateDto{ Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };
        var garageDto = new GarageDto{ Id = 1 };
        var carCreateDto = new CarCreateDto{ Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engineCreateDto, Garage = garageDto };
        
        var engine = new Engine{ Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

        var garage = new Garage{ Id = 1 };

        var car = new Car { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engine, Garage = garage };
        var engineReadDto = new EngineReadDto{ Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

        var carReadDto = new CarReadDto { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engineReadDto, Garage = garageDto };
        
        _mapperMock.Setup(m => m.Map<Car>(carCreateDto)).Returns(car);
        _mapperMock.Setup(m => m.Map<Engine>(engineCreateDto)).Returns(engine);
        _mapperMock.Setup(m => m.Map<Garage>(garageDto)).Returns(garage);
        _logicMock.Setup(l => l.CreateCarAsync(car)).ReturnsAsync(car);
        _mapperMock.Setup(m => m.Map<CarReadDto>(car)).Returns(carReadDto);
        _mapperMock.Setup(m => m.Map<EngineReadDto>(engine)).Returns(engineReadDto);
        _mapperMock.Setup(m => m.Map<GarageDto>(garage)).Returns(garageDto);
        // Act
        var result = await _controller.CreateCar(carCreateDto);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        var createdDto = Assert.IsType<CarReadDto>(createdResult.Value);
        Console.WriteLine(carReadDto.Description);
        Assert.Equal(carReadDto, createdDto);
    }

    [Fact]
    public async Task CreateCar_InvalidCar_ReturnsBadRequest()
    {
        var carCreateDto = new CarCreateDto{ Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 0 };
        var car = new Car();
        var carReadDto = new CarReadDto();
        _mapperMock.Setup(m => m.Map<Car>(carCreateDto)).Returns(car);
        _logicMock.Setup(l => l.CreateCarAsync(car)).Throws(new ArgumentException());
        _mapperMock.Setup(m => m.Map<CarReadDto>(car)).Returns(carReadDto);

        // Act
        var result = await _controller.CreateCar(carCreateDto);
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        //var error = Assert.IsType<String>(badRequestResult.Value);
    }

    [Fact]
    public async Task CreateCar_Exception_ReturnsStatusCode500()
    {
            // Arrange
        var carCreateDto = new CarCreateDto{ Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5
        };
        var car = new Car();
        var carReadDto = new CarReadDto();
        _mapperMock.Setup(m => m.Map<Car>(carCreateDto)).Returns(car);
        _logicMock.Setup(l => l.CreateCarAsync(car)).Throws(new Exception());
        _mapperMock.Setup(m => m.Map<CarReadDto>(car)).Returns(carReadDto);

        // Act
        var result = await _controller.CreateCar(carCreateDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }
    [Fact]
    public async Task GetCarById_WhenCarExists_ReturnsOkResult()
    {
        // Arrange
        var garageDto = new GarageDto{ Id = 1 };
        var engine = new Engine{ Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

        var garage = new Garage{ Id = 1 };

        var car1 = new Car { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engine, Garage = garage };
        var engineReadDto = new EngineReadDto{ Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

        var carReadDto1 = new CarReadDto { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engineReadDto, Garage = garageDto };
        _logicMock.Setup(x => x.GetCarAsync(1)).ReturnsAsync(car1);
        _mapperMock.Setup(x => x.Map<CarReadDto>(car1)).Returns(carReadDto1);
        _mapperMock.Setup(m => m.Map<EngineReadDto>(engine)).Returns(engineReadDto);
        _mapperMock.Setup(m => m.Map<GarageDto>(garage)).Returns(garageDto);

        // Act
        var result = await _controller.GetCarById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsType<CarReadDto>(okResult.Value);
        Assert.Equal(carReadDto1, model);
    }

    [Fact]
    public async Task GetCarById_WhenCarDoesNotExist_ReturnsNotFoundResult()
    {
        // Arrange
        _logicMock.Setup(x => x.GetCarAsync(1)).ReturnsAsync((Car)null);

        // Act
        var result = await _controller.GetCarById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCarById_WhenArgumentExceptionThrown_ReturnsBadRequestResult()
    {
        // Arrange
        _logicMock.Setup(x => x.GetCarAsync(1)).ThrowsAsync(new ArgumentException("Invalid argument"));

        // Act
        var result = await _controller.GetCarById(1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid argument", badRequestResult.Value);
    }

    [Fact]
    public async Task GetCarById_WhenExceptionThrown_ReturnsInternalServerErrorResult()
    {
        // Arrange
        _logicMock.Setup(x => x.GetCarAsync(1)).ThrowsAsync(new Exception("Error message"));

        // Act
        var result = await _controller.GetCarById(1);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }
    [Fact]
    public async Task DeleteCarAsync_ReturnsNoContent_WhenLogicDeletesCar()
    {
        // Arrange
        int id = 1;
        _logicMock.Setup(x => x.DeleteCarAsync(id)).Verifiable();

        // Act
        var result = await _controller.DeleteCarAsync(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _logicMock.Verify();
    }

    [Fact]
    public async Task DeleteCarAsync_ReturnsBadRequest_WhenLogicThrowsArgumentException()
    {
        // Arrange
        int id = 1;
        var message = "Car not found";
        _logicMock.Setup(x => x.DeleteCarAsync(id)).ThrowsAsync(new ArgumentException(message));

        // Act
        var result = await _controller.DeleteCarAsync(id);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(message, badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteCarAsync_ReturnsInternalServerError_WhenLogicThrowsException()
    {
        // Arrange
        int id = 1;
        var exceptionMessage = "Something went wrong";
        _logicMock.Setup(x => x.DeleteCarAsync(id)).ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.DeleteCarAsync(id);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}
