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

    
}