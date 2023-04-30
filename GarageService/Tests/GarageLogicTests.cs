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
        _logic = new GarageLogic(_repositoryMock.Object,_userRepositoryMock.Object,_publishEndPoint.Object);
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

}
