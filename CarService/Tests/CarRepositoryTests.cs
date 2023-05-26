using CarService.Data;
using CarService.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarService.Tests;

public class CarRepositoryTests : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _options;

    public CarRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task CreateCarAsync_ShouldAddCarToContextAndSaveChanges()
    {
        // Arrange
        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);
            var car = new Car{Name = "Car 1", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5};

            // Act
            var result = await repository.CreateCarAsync(car);

            // Assert
            Assert.Equal(car, result);
            Assert.Contains(car, context.Cars);
        }
    }

    [Fact]
    public async Task DeleteCarAsync_ShouldRemoveCarFromContextAndSaveChanges()
    {
        // Arrange
        var carId = 1;
        using (var context = new AppDbContext(_options))
        {
            var car = new Car { Id = carId, Name = "Car 1", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5 };
            context.Cars.Add(car);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);

            // Act
            await repository.DeleteCarAsync(carId);

            // Assert
            Assert.DoesNotContain(context.Cars, c => c.Id == carId);
        }
    }

    [Fact]
    public async Task GetAllCarsAsync_ShouldReturnAllCarsWithGarageAndEngine()
    {
        // Arrange
        using (var context = new AppDbContext(_options))
        {
            var garage = new Garage{Id = 1};
            var engine = new Engine{FuelType="Test fuel", Size = 1.0};
            var cars = new List<Car>
            {
                new Car { Name = "Car 1", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5, Garage = garage, Engine = engine },
                new Car { Name = "Car 2", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5, Garage = garage, Engine = engine },
                new Car { Name = "Car 3", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5, Garage = garage, Engine = engine }
            };
            context.AddRange(cars);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);

            // Act
            var result = await repository.GetAllCarsAsync();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.All(result, car =>
            {
                Assert.NotNull(car.Garage);
                Assert.NotNull(car.Engine);
            });
        }
    }

    [Fact]
    public async Task GetCarAsync_ShouldReturnCarByIdWithGarageAndEngine()
    {
        // Arrange
        var carId = 1;
        using (var context = new AppDbContext(_options))
        {
            var garage = new Garage();
            var engine = new Engine{ FuelType = "Test fuel"};
            var car = new Car { Id = carId,Name = "Car 1", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5, Garage = garage, Engine = engine };
            context.Add(car);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);

            // Act
            var result = await repository.GetCarAsync(carId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(carId, result.Id);
            Assert.NotNull(result.Garage);
            Assert.NotNull(result.Engine);
        }
    }

    [Fact]
    public async Task UpdateCarAsync_ShouldUpdateCarInContextAndSaveChanges()
    {
        // Arrange
        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);
            var car = new Car{Name = "Car 1", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5};
            context.Add(car);
            context.SaveChanges();

            car.Name = "Updated Car";
            
            // Act
            var result = await repository.UpdateCarAsync(car);

            // Assert
            Assert.Equal(car, result);
            Assert.Equal("Updated Car", car.Name);
        }
    }

    [Fact]
    public async Task GetAllCarsAsync_WithCarQuery_ShouldReturnFilteredCarsWithGarageAndEngine()
    {
        // Arrange
        using (var context = new AppDbContext(_options))
        {
            var garage1 = new Garage { Id = 1 };
            var garage2 = new Garage { Id = 2 };
            var engine = new Engine{FuelType = "Test fuel"};
            var cars = new List<Car>
            {
                new Car { Name = "Car 1", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5, Garage = garage1, Engine = engine },
                new Car { Name = "Car 2", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5, Garage = garage2, Engine = engine },
                new Car { Name = "Car 3", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5, Garage = garage1, Engine = engine }
            };
            context.AddRange(cars);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);
            var carQuery = new CarQuery { GarageId = 1, CarName = "Car" };

            // Act
            var result = await repository.GetAllCarsAsync(carQuery);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, car =>
            {
                Assert.Equal(1, car.Garage.Id);
                Assert.True(car.Name.Contains("Car", StringComparison.OrdinalIgnoreCase));
            });
        }
    }

    [Fact]
    public async Task GetCarImageAsync_ShouldReturnCarImageById()
    {
        // Arrange
        var carId = 1;
        using (var context = new AppDbContext(_options))
        {
            var image = new Image{ Data = new byte[]{1,2, 3}};
            var engine = new Engine{FuelType = "Test fuel"};
            var garage = new Garage{};
            var car = new Car { Id = carId, Name = "Car 1", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5, Image = image, Engine = engine, Garage = garage };
            
            await context.AddAsync(car);
            //await context.AddAsync(image);
            await context.SaveChangesAsync();
        }

        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);

            // Act
            var result = await repository.GetCarImageAsync(carId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Image>(result);
        }
    }

    [Fact]
    public async Task CreateCarImageAsync_ShouldAddCarImageToContextAndSaveChanges()
    {
        // Arrange
        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);
            var carImage = new Image{Data = new byte[]{1, 2}};

            // Act
            var result = await repository.CreateCarImageAsync(carImage);

            // Assert
            Assert.Equal(carImage, result);
            Assert.Contains(carImage, context.Images);
        }
    }

    [Fact]
    public async Task UpdateCarWithImageAsync_ShouldUpdateCarWithImageAndSaveChanges()
    {
        // Arrange
        var carId = 1;
        using (var context = new AppDbContext(_options))
        {
            var car = new Car { Id = carId, Name = "Car 1", Description="Test description", Manufacturer="Test Manufacturer", Model="Test Model", Year=2000, Seats=5 };
            context.Cars.Add(car);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(_options))
        {
            var repository = new CarRepository(context);
            var carImage = new Image{Data = new byte[]{1,2 }};

            // Act
            await repository.UpdateCarWithImageAsync(carImage, carId);

            // Assert
            var car = await context.Cars.FindAsync(carId);
            Assert.Equal(carImage, car.Image);
        }
    }

    public void Dispose()
    {

    }
}

