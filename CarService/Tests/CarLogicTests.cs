using Moq;
using CarService.Data;
using CarService.Models;
using MassTransit;
using Contracts;

namespace CarService.Tests;
public class CarLogicTests
{
    private readonly Mock<ICarRepository> _mockCarRepository;
    private readonly Mock<IGarageRepository> _mockGarageRepository;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;
    private readonly CarLogic _carLogic;

    public CarLogicTests()
    {
        _mockCarRepository = new Mock<ICarRepository>();
        _mockGarageRepository = new Mock<IGarageRepository>();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
        _carLogic = new CarLogic(_mockCarRepository.Object, _mockPublishEndpoint.Object, _mockGarageRepository.Object);
    }

    [Fact]
    public async Task CreateCarAsync_ValidCar_CreatesCar()
    {
        // Arrange
        var garage = new Garage { Id = 1 };
        _mockGarageRepository.Setup(x => x.GetGarageAsync(garage.Id)).ReturnsAsync(garage);
        var car = new Car { Name = "Test Car", Description = "Test Description", Manufacturer = "Test Manufacturer", Model = "Test Model", Year = 2023,Seats = 5, Garage = garage, Engine = new Engine { Size = 2.0, FuelType = "Gas", PowerHP = 200, TorqueNM = 350 } };
        _mockCarRepository.Setup(x => x.CreateCarAsync(car)).ReturnsAsync(car);

        // Act
        var result = await _carLogic.CreateCarAsync(car);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(car.Name, result.Name);
        Assert.Equal(car.Description, result.Description);
        Assert.Equal(car.Manufacturer, result.Manufacturer);
        Assert.Equal(car.Model, result.Model);
        Assert.Equal(car.Year, result.Year);
        Assert.Equal(car.Garage, result.Garage);
        Assert.Equal(car.Engine.FuelType, result.Engine.FuelType);
        Assert.Equal(car.Engine.PowerHP, result.Engine.PowerHP);
    }

    [Fact]
    public async Task CreateCarAsync_InvalidCar_ThrowsException()
    {
        // Arrange
        var car = new Car { };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _carLogic.CreateCarAsync(car));
    }

    [Fact]
    public async Task CreateCarAsync_InvalidGarage_ThrowsException()
    {
        // Arrange
        var garageId = 1;
        _mockGarageRepository.Setup(x => x.GetGarageAsync(garageId)).ReturnsAsync((Garage)null);
        var car = new Car { Garage = new Garage { Id = garageId } };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await _carLogic.CreateCarAsync(car));
    }

    [Fact]
    public async Task GetAllCarsAsync_ReturnsAllCars()
    {
        // Arrange
        var cars = new List<Car> { new Car { Id = 1 }, new Car { Id = 2 } };
        _mockCarRepository.Setup(x => x.GetAllCarsAsync()).ReturnsAsync(cars);

        // Act
        var result = await _carLogic.GetAllCarsAsync();

        // Assert
        Assert.Equal(cars, result);
    }

    [Fact]
    public async Task GetAllCarsAsync_WithCarQuery_ReturnsFilteredCars()
    {
        // Arrange
        var carQuery = new CarQuery() { GarageId = 1, CarName = "Car1" };

        var cars = new List<Car>()
        {
            new Car() { Id = 1, Name = "Car1", Garage = new Garage() { Id = 1 } },
            new Car() { Id = 2, Name = "Car2", Garage = new Garage() { Id = 2 } },
            new Car() { Id = 3, Name = "Car3", Garage = new Garage() { Id = 1 } }
        };

        _mockCarRepository.Setup(x => x.GetAllCarsAsync(carQuery)).ReturnsAsync(cars.Where(c => c.Name == "Car1" && c.Garage?.Id == 1));

        // Act
        var result = await _carLogic.GetAllCarsAsync(carQuery);

        // Assert
        Assert.Equal(1, result.Count());
        Assert.Equal("Car1", result.First().Name);
    }
     [Fact]
    public async Task GetCarAsync_WithValidId_ReturnsCar()
    {
        // Arrange
        var car = new Car() { Id = 1 };

        _mockCarRepository.Setup(x => x.GetCarAsync(1)).ReturnsAsync(car);

        // Act
        var result = await _carLogic.GetCarAsync(1);

        // Assert
        Assert.Equal(car, result);
    }

    [Fact]
    public async Task GetCarAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _mockCarRepository.Setup(x => x.GetCarAsync(1)).ReturnsAsync((Car)null);

        // Act
        var result = await _carLogic.GetCarAsync(1);

        // Assert
        Assert.Null(result);
    }
}