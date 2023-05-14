using Xunit;
using Moq;
using AutoMapper;
using UserService.Controllers;
using UserService.Logic;

namespace UserService.Tests;

public class UsersControllerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserLogic> _logicMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _logicMock = new Mock<IUserLogic>();
        _controller = new UsersController( _logicMock.Object, _mapperMock.Object);
    }
}

