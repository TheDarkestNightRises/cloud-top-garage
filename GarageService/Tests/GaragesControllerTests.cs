using Xunit;
using Moq;
using AutoMapper;
using GarageService.Application.LogicContracts;
using Carservice.Controllers;
using GarageService.Dtos;
using GarageService.Models;
using Microsoft.AspNetCore.Mvc;

namespace GarageService.Tests;

public class GaragesControllerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IGarageLogic> _logicMock;
    private readonly GaragesController _controller;

    public GaragesControllerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _logicMock = new Mock<IGarageLogic>();
        _controller = new GaragesController(_mapperMock.Object, _logicMock.Object);
    }

    [Fact]
    public async Task GetAllGaragesAsync_ReturnsOkResult_WhenGaragesExist()
    {
        var garageQueryDto = new GarageQueryDto();
        var garageQuery = new GarageQuery();
        var garages = new List<Garage>();
        var garageReadDtos = new List<GarageReadDto>();

        _mapperMock.Setup(m => m.Map<GarageQuery>(garageQueryDto)).Returns(garageQuery);
        _logicMock.Setup(l => l.GetAllGaragesAsync(garageQuery)).ReturnsAsync(garages);
        _mapperMock.Setup(m => m.Map<IEnumerable<GarageReadDto>>(garages)).Returns(garageReadDtos);

        var result = await _controller.GetAllGaragesAsync(garageQueryDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IEnumerable<GarageReadDto>>(okResult.Value);
        Assert.Equal(garageReadDtos, model);
    }
}