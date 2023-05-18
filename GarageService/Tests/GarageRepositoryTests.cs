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

        // Create a garage
        var garageId = 1;
        var car1 = new Car { };
        var car2 = new Car { };
        var user1 = new User { };
        var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
        var garage = new Garage { Name = "Main Garage", Capacity = 5, Location = location1, User = user1, Cars = new List<Car> { car1, car2 } };
        context.Garages.Add(garage);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetGarageAsync(garageId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(garageId, result.Id);

    }
}
