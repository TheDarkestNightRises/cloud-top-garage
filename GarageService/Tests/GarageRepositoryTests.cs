using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using GarageService.Data;
using GarageService.Models;

public class GarageRepositoryTests
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;

    public GarageRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
    }

    // ----------------------- GET GARAGE BY ID--------------------------------------------------

    [Fact]
    public async Task GetGarageAsync_ReturnsNull_WhenGarageDoesNotExist()
    {
        // Arrange
        var garageId = 1;
        var context = new AppDbContext(_dbContextOptions);
        var repository = new GarageRepository(context);

        // Act
        var result = await repository.GetGarageAsync(garageId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetGarageAsync_ReturnsGarage_WhenGarageExists()
    {
        // Arrange
        var context = new AppDbContext(_dbContextOptions);
        var repository = new GarageRepository(context);

        // Create garage
        var garageId = 1;
        var car1 = new Car { };
        var car2 = new Car { };
        var user1 = new User { };
        var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
        var garage = new Garage { Name = "Garage", Capacity = 5, Location = location1, User = user1, Cars = new List<Car> { car1, car2 } };
        context.Garages.Add(garage);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetGarageAsync(garageId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(garageId, result.Id);

    }

    [Fact]
    public async Task GetGarageAsync_ReturnsNull_WhenInvalidGarageId()
    {
        // Arrange
        var context = new AppDbContext(_dbContextOptions);
        var repository = new GarageRepository(context);
        var garageId = -1;

        // Act
        var result = await repository.GetGarageAsync(garageId);

        // Assert
        Assert.Null(result);
    }

    // ----------------------- GET ALL GARAGES--------------------------------------------------

    [Fact]
    public async Task GetAllGaragesAsync_ReturnsGarages_WhenValidUserId()
    {
        // Arrange
        var context = new AppDbContext(_dbContextOptions);
        var repository = new GarageRepository(context);

        var car1 = new Car { };
        var car2 = new Car { };

        var user1 = new User { Id = 1 };
        context.Users.Add(user1);

        var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
        var garage = new Garage { Name = "Garage", Capacity = 5, Location = location1, User = user1, Cars = new List<Car> { car1, car2 } };
        context.Garages.Add(garage);

        await context.SaveChangesAsync();

        var garageQuery = new GarageQuery { UserId = 1 };

        // Act
        var result = await repository.GetAllGaragesAsync(garageQuery);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetAllGaragesAsync_ReturnsAllGarages_WhenNullUserId()
    {
        // Arrange
        var context = new AppDbContext(_dbContextOptions);
        var repository = new GarageRepository(context);

        var user1 = new User { Id = 1 };
        var user2 = new User { Id = 2 };
        context.Users.AddRange(user1, user2);

        var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
        var garage1 = new Garage { Name = "Garage 1", Capacity = 5, Location = location1, User = user1 };
        var garage2 = new Garage { Name = "Garage 2", Capacity = 5, Location = location1, User = user2 };
        context.Garages.AddRange(garage1, garage2);
        await context.SaveChangesAsync();

        var garageQuery = new GarageQuery { UserId = null };

        // Act
        var result = await repository.GetAllGaragesAsync(garageQuery);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }
}
