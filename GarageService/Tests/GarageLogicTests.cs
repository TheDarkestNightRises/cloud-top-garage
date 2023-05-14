using Xunit;
using Moq;
using GarageService.Data;
using GarageService.Application.LogicContracts;
using GarageService.Models;
using MassTransit;

public class GarageLogicTests
{
    private readonly Mock<IGarageRepository> _repositoryMock;
    private readonly Mock<UserRepository> _userRepositoryMock;
    private readonly Mock<IPublishEndpoint> _publishEndPoint;
    private readonly IGarageLogic _logic;

    public GarageLogicTests()
    {
        _repositoryMock = new Mock<IGarageRepository>();
        _userRepositoryMock = new Mock<UserRepository>();
        _publishEndPoint = new Mock<IPublishEndpoint>();
        _logic = new GarageLogic(_repositoryMock.Object, _userRepositoryMock.Object, _publishEndPoint.Object);
    }

    // ----------------------------- DELETE GARAGE --------------------------------------------
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

    // ----------------------------- GET ALL GARAGES --------------------------------------------

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


}
