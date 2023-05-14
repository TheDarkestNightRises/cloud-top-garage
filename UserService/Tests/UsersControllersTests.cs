using Xunit;
using Moq;
using AutoMapper;
using UserService.Controllers;
using UserService.Logic;
using UserService.Models;
using UserService.Dtos;
using Microsoft.AspNetCore.Mvc;

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
        _controller = new UsersController(_logicMock.Object, _mapperMock.Object);
    }
    // ----------------------------- GET ALL USERS --------------------------------------------
    [Fact]
    public async Task GetAllUsers_ReturnsOkResult_WhenUsersExists()
    {
        // Arrange
        var users = new List<User> { new User { Name = "John Doe", Email = "johndoe@email.com", Password = "123", Role = "User", Age = "18", Phone = "123" } };
        var usersMapped = new List<UserReadDto> { new UserReadDto { Name = "John Doe", Email = "johndoe@email.com", Age = "18", Phone = "123" } };


        _logicMock.Setup(ul => ul.GetAllUsersAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(usersMapped);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserReadDto>>(okResult.Value);
        Assert.Equal(usersMapped, returnedUsers);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsNotFoundResult_WhenUsersDontExist()
    {
        // Arrange
        List<User> users = null;
        List<UserReadDto> usersMapped = null;


        _logicMock.Setup(ul => ul.GetAllUsersAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(usersMapped);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}


