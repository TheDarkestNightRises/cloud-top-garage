using CarService.Data;
using CarService.Models;
using Contracts;
using MassTransit;
using Moq;

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
    public async Task CreateCarAsync_ReturnsCreatedCar_WhenCarValid()
    {
        // Arrange
        var garage = new Garage { Id = 1 };
        _mockGarageRepository.Setup(x => x.GetGarageAsync(garage.Id)).ReturnsAsync(garage);
        var car = new Car { Name = "Test Car", Description = "Test Description", Manufacturer = "Test Manufacturer", Model = "Test Model", Year = 2023, Seats = 5, Garage = garage, Engine = new Engine { Size = 2.0, FuelType = "Gas", PowerHP = 200, TorqueNM = 350 } };
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
    public async Task CreateCarAsync_ThrowsExceptionWithCorrectMessage_WhenCarInvalid()
    {
        // Arrange invalid name
        var car = new Car { Name = "", Description = "Test Description", Manufacturer = "Test Manufacturer", Model = "Test Model", Year = 2023, Seats = 5, Garage = new Garage { Id = 1 }, Engine = new Engine { Size = 2.0, FuelType = "Gas", PowerHP = 200, TorqueNM = 350 } };


        // Act & Assert
        ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _carLogic.CreateCarAsync(car));
        Assert.Equal("The Name field is required.", ex.Message);
    }
    [Fact]
    public async Task CreateCarAsync_ThrowsExceptionWithCorrectMessage_WhenEngineInvalid()
    {
        // Arrange
        var car = new Car { Name = "Test Name", Description = "Test Description", Manufacturer = "Test Manufacturer", Model = "Test Model", Year = 2023, Seats = 5, Garage = new Garage { Id = 1 }, Engine = new Engine { Size = 0, FuelType = "Gas", PowerHP = 200, TorqueNM = 350 } };


        // Act & Assert
        ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _carLogic.CreateCarAsync(car));
        Assert.Equal("The engine size must be between 0.1 and 10.", ex.Message);
    }
    [Fact]
    public async Task CreateCarAsync_ThrowsExceptionWithCorrectMessage_WhenEngineIsNull()
    {
        // Arrange
        var car = new Car { Name = "Test Name", Description = "Test Description", Manufacturer = "Test Manufacturer", Model = "Test Model", Year = 2023, Seats = 5, Garage = new Garage { Id = 1 } };


        // Act & Assert
        ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(async () => await _carLogic.CreateCarAsync(car));
        Assert.Equal("Engine must be specified.", ex.Message);
    }
    [Fact]
    public async Task CreateCarAsync_InvalidGarage_ThrowsException()
    {
        // Arrange
        var garageId = 1;
        _mockGarageRepository.Setup(x => x.GetGarageAsync(garageId)).ReturnsAsync(null as Garage);
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
    public async Task GetAllCarsAsync_ReturnsFilteredCars_WithCarQuery()
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
        Assert.Single(result);
        Assert.Equal("Car1", result.First().Name);
    }
    [Fact]
    public async Task GetCarAsync_ReturnsCarById_WhenCarExists()
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
    public async Task GetCarAsync_ReturnsNull_WhenInvalidId()
    {
        // Arrange
        _mockCarRepository.Setup(x => x.GetCarAsync(1)).ReturnsAsync((Car)null);

        // Act
        var result = await _carLogic.GetCarAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteCarAsync_ThrowsArgumentException_WhenCarDoesNotExist()
    {
        // Arrange
        int carId = 1;
        Car car = null;
        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync(car);

        // Act + Assert
        ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => _carLogic.DeleteCarAsync(carId));
        Assert.Equal(ex.Message, $"Car with id {carId} not found");
    }

    [Fact]
    public async Task DeleteCarAsync_DeletesCar_WhenCarExists()
    {
        // Arrange
        int carId = 1;
        Car car = new Car { Id = carId };
        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync(car);
        _mockCarRepository.Setup(repo1 => repo1.DeleteCarAsync(carId)).Verifiable();
        // Act
        await _carLogic.DeleteCarAsync(carId);

        // Assert
        _mockCarRepository.Verify(repo => repo.DeleteCarAsync(carId), Times.Once);
    }
    [Fact]
    public async Task GetCarImageAsync_ThrowsArgumentException_WhenCarDoesNotExist()
    {
        // Arrange
        int carId = 1;
        Car car = null;
        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync(car);

        // Act + Assert
        ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => _carLogic.GetCarImageAsync(carId));
        Assert.Equal(ex.Message, $"Car with id {carId} not found");
    }

    [Fact]
    public async Task GetCarImageAsync_ReturnsCarImage_WhenCarExistsAndHasImage()
    {
        // Arrange
        int carId = 1;
        Car car = new Car { Id = carId };
        Image carImage = new Image();
        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync(car);
        _mockCarRepository.Setup(repo => repo.GetCarImageAsync(carId)).ReturnsAsync(carImage);

        // Act
        var result = await _carLogic.GetCarImageAsync(carId);

        // Assert
        Assert.Equal(carImage, result);
    }

    [Fact]
    public async Task UpdateCarAsync_ThrowsArgumentException_WhenCarDOesNotExist()
    {
        // Arrange
        int carId = 1;
        Car carToUpdate = new Car { Id = carId };
        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync((Car)null);

        // Act & Assert
        ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => _carLogic.UpdateCarAsync(carToUpdate));
        Assert.Equal(ex.Message, $"Car with id {carId} not found");
    }

    [Fact]
    public async Task UpdateCarAsync_ThrowsArgumentException_WhenGarageDoesNotExist()
    {
        // Arrange
        int carId = 1;
        int garageId = 2;
        Car carToUpdate = new Car { Id = carId, Garage = new Garage { Id = garageId } };
        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync(carToUpdate);
        _mockGarageRepository.Setup(repo => repo.GetGarageAsync(2)).ReturnsAsync((Garage)null);

        // Act & Assert
        ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => _carLogic.UpdateCarAsync(carToUpdate));
        Assert.Equal(ex.Message, $"Garage with id {garageId} not found");
    }

    [Fact]
    public async Task UpdateCarAsync_UpdatesCar_WhenCarAndGarageExist()
    {
        // Arrange
        int carId = 1;
        int newGarageId = 2;
        int oldGarageId = 3;
        Car carToUpdate = new Car { Id = carId, Garage = new Garage { Id = newGarageId } };
        Car carFound = new Car { Id = carId, Garage = new Garage { Id = oldGarageId } };
        Garage newGarage = new Garage { Id = newGarageId };

        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync(carFound);
        _mockGarageRepository.Setup(repo => repo.GetGarageAsync(newGarageId)).ReturnsAsync(newGarage);
        // Act
        Car updatedCar = await _carLogic.UpdateCarAsync(carToUpdate);

        // Assert
        Assert.Equal(newGarage, updatedCar.Garage);
        //_mockCarRepository.Verify(repo => repo.UpdateCarAsync(updatedCar), Times.Once);
    }
    [Fact]
    public async Task CreateCarImage_ReturnsCreatedImage_WhenCarExists()
    {
        // Arrange
        int carId = 1;
        var car = new Car { Id = carId };
        var image = new Image { Id = 1, Data = new byte[] { 0x01, 0x02 } };
        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync(car);
        _mockCarRepository.Setup(repo => repo.CreateCarImageAsync(image)).ReturnsAsync(image);
        _mockCarRepository.Setup(repo => repo.UpdateCarWithImageAsync(image, carId)).Verifiable();

        // Act
        var result = await _carLogic.CreateCarImage(image, carId);

        // Assert
        Assert.Equal(image, result);
        _mockCarRepository.Verify(repo => repo.UpdateCarWithImageAsync(image, carId), Times.Once);
    }

    [Fact]
    public async Task CreateCarImage_ThrowsException_WhenCarDoesNotExist()
    {
        // Arrange
        int carId = 1;
        _mockCarRepository.Setup(repo => repo.GetCarAsync(carId)).ReturnsAsync((Car)null);
        var image = new Image { Id = 1, Data = new byte[] { 0x01, 0x02 } };

        // Act & Assert
        ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => _carLogic.CreateCarImage(image, carId));
        Assert.Equal(ex.Message, $"Car with id {carId} not found");
        _mockCarRepository.Verify(repo => repo.CreateCarImageAsync(image), Times.Never);
        _mockCarRepository.Verify(repo => repo.UpdateCarWithImageAsync(image, carId), Times.Never);
    }
}
