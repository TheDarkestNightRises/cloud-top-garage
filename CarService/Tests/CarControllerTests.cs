using Xunit;
using Moq;
using AutoMapper;
using Application.LogicContracts;
using Carservice.Controllers;
using CarService.Dtos;
using CarService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tests;

public class CarsControllerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICarLogic> _logicMock;
    private readonly CarsController _controller;

    public CarsControllerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _logicMock = new Mock<ICarLogic>();
        _controller = new CarsController(_mapperMock.Object, _logicMock.Object);
    }

    
}
