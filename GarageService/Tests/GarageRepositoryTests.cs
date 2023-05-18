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
}
