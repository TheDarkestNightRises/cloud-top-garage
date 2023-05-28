using GarageService.Data;
using GarageService.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class GarageRepositoryTests : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _options;

    public GarageRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    // ----------------------- GET GARAGE BY ID--------------------------------------------------

    [Fact]
    public async Task GetGarageAsync_ReturnsNull_WhenGarageDoesNotExist()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
            var garageId = 1;
            var repository = new GarageRepository(context);

            // Act
            var result = await repository.GetGarageAsync(garageId);

            // Assert
            Assert.Null(result);
        }
    }

    [Fact]
    public async Task GetGarageAsync_ReturnsGarage_WhenGarageExists()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
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
    }

    [Fact]
    public async Task GetGarageAsync_ReturnsNull_WhenInvalidGarageId()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
            var repository = new GarageRepository(context);
            var garageId = -1;

            // Act
            var result = await repository.GetGarageAsync(garageId);

            // Assert
            Assert.Null(result);
        }
    }

    // ----------------------- GET ALL GARAGES--------------------------------------------------

    [Fact]
    public async Task GetAllGaragesAsync_ReturnsGarages_WhenValidUserId()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
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
    }

    [Fact]
    public async Task GetAllGaragesAsync_ReturnsAllGarages_WhenNullUserId()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
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

    [Fact]
    public async Task GetAllGaragesAsync_ReturnsEmptyList_WhenInvalidUserId()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
            var repository = new GarageRepository(context);

            var user1 = new User { Id = 1 };
            var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
            var garage1 = new Garage { Name = "Garage 1", Capacity = 5, Location = location1, User = user1 };
            context.Garages.Add(garage1);
            await context.SaveChangesAsync();

            var garageQuery = new GarageQuery { UserId = -1 };
            // Act
            var result = await repository.GetAllGaragesAsync(garageQuery);

            // Assert
            Assert.Empty(result);
        }
    }

    // ----------------------- UPDATE GARAGE --------------------------------------------------

    [Fact]
    public async Task UpdateGarageAsync_OneField_UpdatesGarageInDatabase()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
            var repository = new GarageRepository(context);

            var car1 = new Car { };
            var car2 = new Car { };
            var user1 = new User { };
            var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
            var garage = new Garage { Name = "Garage", Capacity = 5, Location = location1, User = user1, Cars = new List<Car> { car1, car2 } };
            context.Garages.Add(garage);

            await context.SaveChangesAsync();

            // Modify the garage
            garage.Name = "Updated Garage";

            // Act
            await repository.UpdateGarageAsync(garage);

            // Retrieve the garage from the database
            var updatedGarage = await context.Garages.FindAsync(1);

            // Assert
            Assert.Equal(garage.Name, updatedGarage?.Name);
        }
    }

    [Fact]
    public async Task UpdateGarageAsync_TwoFields_UpdatesGarageInDatabase()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
            var repository = new GarageRepository(context);

            var car1 = new Car { };
            var car2 = new Car { };
            var user1 = new User { };
            var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
            var garage = new Garage { Name = "Garage", Capacity = 5, Location = location1, User = user1, Cars = new List<Car> { car1, car2 } };
            context.Garages.Add(garage);

            await context.SaveChangesAsync();

            // Modify the garage
            garage.Name = "Updated Garage";
            garage.Capacity = 6;

            // Act
            await repository.UpdateGarageAsync(garage);

            // Retrieve the garage from the database
            var updatedGarage = await context.Garages.FindAsync(1);

            // Assert
            Assert.Equal(garage.Name, updatedGarage?.Name);
            Assert.Equal(garage.Capacity, updatedGarage?.Capacity);
        }
    }

    // ----------------------- DELETE GARAGE --------------------------------------------------

    [Fact]
    public async Task DeleteGarageAsync_DeletesExistingGarage()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
            var repository = new GarageRepository(context);
            int garageId = 1;
            var car1 = new Car { };
            var car2 = new Car { };
            var user1 = new User { };
            var location1 = new Location { Latitude = 55.862656, Longitude = 9.837616 };
            var garage = new Garage { Name = "Garage", Capacity = 5, Location = location1, User = user1, Cars = new List<Car> { car1, car2 } };
            context.Garages.Add(garage);

            await context.SaveChangesAsync();

            // Act
            await repository.DeleteGarageAsync(garageId);

            // Assert
            var deletedGarage = await context.Garages.FindAsync(garage.Id);
            Assert.Null(deletedGarage);
        }
    }

    [Fact]
    public async Task DeleteGarageAsync_WhenGarageDoesNotExist()
    {
        using (var context = new AppDbContext(_options))
        {
            // Arrange
            var repository = new GarageRepository(context);

            var nonExistentGarageId = 1;

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteGarageAsync(nonExistentGarageId));
        }
    }

    public void Dispose()
    {

    }
}
