using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using UserService.Controllers;
using UserService.Dtos;
using UserService.Logic;
using UserService.Models;
using Xunit;

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
    public async Task GetAllUsersAsync_WhenUsersExists_ReturnsOkResult()
    {
        // Arrange
        var users = new List<User> { new User { Name = "John Doe", Email = "johndoe@email.com", Password = "123abcdefg", Role = "User", Age = 18, Phone = "123" } };
        var usersMapped = new List<UserReadDto> { new UserReadDto { Name = "John Doe", Email = "johndoe@email.com", Age = 18, Phone = "123" } };


        _logicMock.Setup(ul => ul.GetAllUsersAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(usersMapped);

        // Act
        var result = await _controller.GetAllUsersAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserReadDto>>(okResult.Value);
        Assert.Equal(usersMapped, returnedUsers);
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenUsersDontExist_ReturnsNotFoundResult()
    {
        // Arrange
        List<User> users = null;
        List<UserReadDto> usersMapped = null;


        _logicMock.Setup(ul => ul.GetAllUsersAsync()).ReturnsAsync(users);
        _mapperMock.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(usersMapped);

        // Act
        var result = await _controller.GetAllUsersAsync();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }


    // ----------------------------- UPDATE USER --------------------------------------------

    [Fact]
    public async Task UpdateUserAsync_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDto { Name = "John Doe", Email = "johndoe@email.com", Password = "123abcdefg", Age = 18, Phone = "123" };
        var userToUpdate = new User { Name = "John Doe", Email = "johndoe@email.com", Password = "123abcdefg", Age = 18, Phone = "123" };

        _mapperMock.Setup(m => m.Map<User>(userUpdateDto)).Returns(userToUpdate);

        // Act
        var result = await _controller.UpdateUserAsync(userUpdateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    // ----------------------------- CREATE USER --------------------------------------------

    [Fact]
    public async Task CreateUserAsync_WhenValidUser_ReturnsCreated()
    {
        // Arrange
        var userCreateDto = new UserCreateDto { Name = "John Doe", Email = "johndoe@email.com", Password = "123abcdefg", Age = 18, Phone = "123" };
        var user = new User { Name = "John Doe", Email = "johndoe@email.com", Password = "123abcdefg", Role = "User", Age = 18, Phone = "123" };
        var userCreated = new User { Id = 1, Name = "John Doe", Email = "johndoe@email.com", Password = "123abcdefg", Role = "User", Age = 18, Phone = "123" };
        var userReadDto = new UserReadDto { Id = 1, Name = "John Doe", Email = "johndoe@email.com", Age = 18, Phone = "123" };

        _mapperMock.Setup(mapper => mapper.Map<User>(userCreateDto)).Returns(user);
        _logicMock.Setup(userLogic => userLogic.CreateUserAsync(user)).ReturnsAsync(userCreated);
        _mapperMock.Setup(mapper => mapper.Map<UserReadDto>(userCreated)).Returns(userReadDto);

        // Act
        var result = await _controller.CreateUserAsync(userCreateDto);

        // Assert
        Console.WriteLine(result);
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal("/users/1", $"/users/{userReadDto.Id}");
        Assert.Equal(userReadDto, createdResult.Value);

    }

    // ----------------------------- GET USERS BY EMAIL --------------------------------------------

    [Fact]
    public async Task GetUserByEmailAsync_ReturnsOkResult_WhenUserExists()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User { Email = email, Name = "Test User" };
        var userReadDto = new UserReadDto { Email = email, Name = "Test User" };

        _logicMock.Setup(ul => ul.GetUserByEmailAsync(email)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<UserReadDto>(user)).Returns(userReadDto);

        // Act
        var result = await _controller.GetUserByEmailAsync(email);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUser = Assert.IsType<UserReadDto>(okResult.Value);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";
        _logicMock.Setup(ul => ul.GetUserByEmailAsync(email)).ReturnsAsync((User)null);
        // Act
        var result = await _controller.GetUserByEmailAsync(email);
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}

