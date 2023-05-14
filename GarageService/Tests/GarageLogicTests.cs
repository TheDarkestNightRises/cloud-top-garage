using Xunit;
using Moq;
using GarageService.Data;
using GarageService.Application.LogicContracts;
using GarageService.Models;
using MassTransit;
using Contracts;

public class GarageLogicTests
{
    private readonly Mock<IGarageRepository> _repositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPublishEndpoint> _publishEndPoint;
    private readonly IGarageLogic _logic;

    public GarageLogicTests()
    {
        _repositoryMock = new Mock<IGarageRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _publishEndPoint = new Mock<IPublishEndpoint>();
        _logic = new GarageLogic(_repositoryMock.Object, _userRepositoryMock.Object, _publishEndPoint.Object);
    }

    // ----------------------------- DELETE GARAGE -------------------------------------------- //
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteGarageAsync_ThrowsException_WhenGarageNotFound(int id)
    {
        _repositoryMock.Setup(r => r.GetGarageAsync(id)).ReturnsAsync((Garage)null);

        await Assert.ThrowsAsync<Exception>(() => _logic.DeleteGarageAsync(id));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task DeleteGarageAsync_CallsRepository_WhenGarageExists(int id)
    {
        // Arrange
        var garage = new Garage { Id = id };
        _repositoryMock.Setup(r => r.GetGarageAsync(id)).ReturnsAsync(garage);

        // Act
        await _logic.DeleteGarageAsync(id);

        // Assert
        _repositoryMock.Verify(r => r.DeleteGarageAsync(id));
    }

    // ----------------------------- GET ALL GARAGES -------------------------------------------- //

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public async Task GetAllGaragesAsync_ReturnsAllGarages(int garageCount)
    {
        // Arrange
        var garageQuery = new GarageQuery();
        var expectedGarages = GenerateGarageList(garageCount);
        _repositoryMock.Setup(repo => repo.GetAllGaragesAsync(garageQuery)).ReturnsAsync(expectedGarages);

        // Act
        var result = await _logic.GetAllGaragesAsync(garageQuery);

        // Assert
        Assert.Equal(expectedGarages, result);
    }

    private List<Garage> GenerateGarageList(int count)
    {
        var garages = new List<Garage>();

        for (int i = 1; i <= count; i++)
        {
            garages.Add(new Garage { Id = i, Name = $"Garage {i}", Capacity = 5, User = null, Cars = null });
        }

        return garages;
    }

    // ----------------------------- GET GARAGE -------------------------------------------- //

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetGarageAsync_ReturnsGarage_WhenGarageExists(int id)
    {
        // Arrange
        var expectedGarage = new Garage { Id = id, Name = "Garage 1" };
        _repositoryMock.Setup(repo => repo.GetGarageAsync(id)).ReturnsAsync(expectedGarage);

        // Act
        var result = await _logic.GetGarageAsync(id);

        // Assert
        Assert.Equal(expectedGarage, result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetGarageAsync_ReturnsNull_WhenGarageDoesNotExist(int id)
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetGarageAsync(id)).ReturnsAsync((Garage)null);

        // Act
        var result = await _logic.GetGarageAsync(id);

        // Assert
        Assert.Null(result);
    }

    // ----------------------------- CREATE GARAGE -------------------------------------------- //

    [Theory]
    [InlineData(1, "Garage 1")]
    [InlineData(2, "Garage 2")]
    public async Task CreateGarageAsync_WhenValidInput_CreatesGarage(int userId, string garageName)
    {
        // Arrange
        var garage = new Garage { Id = 1, Name = garageName, User = new User { Id = userId }, Capacity = 5 };

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User());
        _repositoryMock.Setup(repo => repo.CreateGarageAsync(garage)).ReturnsAsync(garage);

        // Act
        var result = await _logic.CreateGarageAsync(garage);

        // Assert
        Assert.Equal(garage, result);
        _repositoryMock.Verify(repo => repo.CreateGarageAsync(garage), Times.Once);
    }


    [Theory]
    [InlineData(1, "Garage 1")]
    [InlineData(2, "Garage 2")]
    public async Task CreateGarageAsync_WhenInvalidInput_ThrowsInvalidCapacityException(int userId, string garageName)
    {
        // Arrange
        var garage = new Garage { Id = 1, Name = garageName, User = new User { Id = userId }, Capacity = -5 };

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _logic.CreateGarageAsync(garage));

        // Verify
        _userRepositoryMock.Verify(repo => repo.GetUserByIdAsync(It.IsAny<int>()), Times.Never);
        _repositoryMock.Verify(repo => repo.CreateGarageAsync(It.IsAny<Garage>()), Times.Never);
    }
}
