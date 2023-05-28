using System.ComponentModel.DataAnnotations;
using Application.LogicContracts;
using AutoMapper;
using Carservice.Controllers;
using CarService.Dtos;
using CarService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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
    public async Task GetAllCarsAsync_ReturnsOkObjectResultWithREadDto_WhenCarsExist()
    {
        // Arrange
        var carQueryDto = new CarQueryDto();
        var carQuery = new CarQuery();

        //car1
        var garageDto = new GarageDto { Id = 1 };
        var engine = new Engine { Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

        var garage = new Garage { Id = 1 };

        var car1 = new Car { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engine, Garage = garage };

        var engineReadDto = new EngineReadDto { Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

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
        var garageDto = new GarageDto { Id = 1 };

        var carUpdateDto = new CarUpdateDto { Id = 1, Garage = garageDto };
        var garage = new Garage { Id = 1 };
        var car = new Car { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Garage = garage };
        _mapperMock.Setup(x => x.Map<Car>(carUpdateDto)).Returns(car);
        _mapperMock.Setup(m => m.Map<Garage>(garageDto)).Returns(garage);
        _logicMock.Setup(x => x.UpdateCarAsync(car)).ReturnsAsync(car);

        // Act
        var result = await _controller.UpdateCarAsync(carUpdateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _logicMock.Verify(logic => logic.UpdateCarAsync(car), Times.Once);
    }

    [Fact]
    public async Task CreateCar_ReturnsCreatedWithCorrectObject_WhenCarIsValid()
    {
        // Arrange
        var engineCreateDto = new EngineCreateDto { Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };
        var garageDto = new GarageDto { Id = 1 };
        var carCreateDto = new CarCreateDto { Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engineCreateDto, Garage = garageDto };

        var engine = new Engine { Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

        var garage = new Garage { Id = 1 };

        var car = new Car { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engine, Garage = garage };
        var engineReadDto = new EngineReadDto { Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

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
    public async Task GetCarById_ReturnsOkResultWithCorrectCarDto_WhenCarExists()
    {
        // Arrange
        var garageDto = new GarageDto { Id = 1 };
        var engine = new Engine { Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

        var garage = new Garage { Id = 1 };

        var car1 = new Car { Id = 1, Name = "Test car", Description = "Test description", Manufacturer = "Test manufacturer", Model = "Test model", Year = 2022, Seats = 5, Engine = engine, Garage = garage };
        var engineReadDto = new EngineReadDto { Id = 1, Size = 1.4, FuelType = "Diesel", PowerHP = 100, TorqueNM = 400 };

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
    public async Task GetCarById_ReturnsNotFoundResult_WhenCarDoesNotExist()
    {
        // Arrange
        _logicMock.Setup(x => x.GetCarAsync(1)).ReturnsAsync((Car)null);

        // Act
        var result = await _controller.GetCarById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteCarAsync_ReturnsNoContent_WhenCarIsDeleted()
    {
        // Arrange
        int id = 1;
        _logicMock.Setup(x => x.DeleteCarAsync(id)).Verifiable();

        // Act
        var result = await _controller.DeleteCarAsync(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _logicMock.Verify(logic => logic.DeleteCarAsync(id), Times.Once);
    }

    [Fact]
    public async Task CreateCarImage_ReturnsFileContentResultWithCreatedImage_WhenImageValid()
    {
        // Arrange
        int carId = 1;
        var imageBytes = new byte[] { 1, 2, 3 };
        var imageFile = new FormFile(new MemoryStream(imageBytes), 0, imageBytes.Length, "image/jpg", "car.jpg");

        var carImage = new Image { Data = imageBytes };
        var createdImage = new Image { Data = imageBytes };

        _logicMock.Setup(x => x.CreateCarImage(carImage, carId)).ReturnsAsync(createdImage);
        Assert.Equal(carImage.Data, createdImage.Data);
        // Act
        var result = await _controller.CreateCarImage(carId, imageFile);

        // Assert
        var fileContentResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("image/jpeg", fileContentResult.ContentType);
        Assert.Equal(createdImage.Data, fileContentResult.FileContents);
    }
  
    [Fact]
    public async Task GetCarImage_ReturnsFileContentResultWithImage_WhenImageExists()
    {
        // Arrange
        const int id = 123;
        var imageData = new byte[] { 0x01, 0x02, 0x03 };
        var carImage = new Image { Data = imageData };
        _logicMock.Setup(x => x.GetCarImageAsync(id)).ReturnsAsync(carImage);

        // Act
        var result = await _controller.GetCarImage(id);

        // Assert
        var fileContentResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("image/jpeg", fileContentResult.ContentType);
        Assert.Equal(imageData, fileContentResult.FileContents);
    }

    [Fact]
    public async Task GetCarImage_ReturnsNotFound_WhenImageDoesNotExist()
    {
        // Arrange
        const int id = 123;
        _logicMock.Setup(x => x.GetCarImageAsync(id)).ReturnsAsync(null as Image);

        // Act
        var result = await _controller.GetCarImage(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
