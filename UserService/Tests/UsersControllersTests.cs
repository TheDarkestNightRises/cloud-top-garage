using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    [Fact]
    public async Task GetAllUsersAsync_WhenException_ReturnsInternalServerErrors()
    {
        // Arrange
        List<User> users = new List<User>();
        List<UserReadDto> usersMapped = new List<UserReadDto>();
        var emsg = "Some error occurred.";


        _logicMock.Setup(ul => ul.GetAllUsersAsync()).ThrowsAsync(new Exception(emsg));
        _mapperMock.Setup(m => m.Map<IEnumerable<UserReadDto>>(users)).Returns(usersMapped);

        // Act
        var result = await _controller.GetAllUsersAsync();


        // Assert
        var errorResponse = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, errorResponse.StatusCode);
        Assert.Equal(emsg, errorResponse.Value);
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
    [Fact]
    public async Task UpdateUserAsync_WhenArgumentException_ReturnsBadRequest()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDto { };
        var userToUpdate = new User { };
        var emsg = "Invalid user update.";

        _mapperMock.Setup(m => m.Map<User>(userUpdateDto)).Returns(userToUpdate);
        _logicMock.Setup(ul => ul.UpdateUserAsync(userToUpdate)).ThrowsAsync(new ArgumentException(emsg));

        // Act
        var result = await _controller.UpdateUserAsync(userUpdateDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(emsg, badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenException_ReturnsInternalServerError()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDto { };
        var exceptionMessage = "An error occurred.";
        var userToUpdate = new User { };

        _mapperMock.Setup(m => m.Map<User>(userUpdateDto)).Returns(userToUpdate);
        _logicMock.Setup(ul => ul.UpdateUserAsync(userToUpdate)).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _controller.UpdateUserAsync(userUpdateDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal(exceptionMessage, statusCodeResult.Value);
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

    [Fact]
    public async Task CreateUserAsync_WhenArgumentException_ReturnsBadRequest()
    {
        // Arrange
        var userCreateDto = new UserCreateDto { };
        var exceptionMessage = "Invalid user create.";

        _mapperMock.Setup(mapper => mapper.Map<User>(userCreateDto)).Returns(It.IsAny<User>());
        _logicMock.Setup(userLogic => userLogic.CreateUserAsync(It.IsAny<User>())).Throws(new ArgumentException(exceptionMessage));

        // Act
        var result = await _controller.CreateUserAsync(userCreateDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(exceptionMessage, badRequestResult.Value);
    }

    [Fact]
    public async Task CreateUserAsync_WhenException_ReturnsInternalServerError()
    {
        // Arrange
        var userCreateDto = new UserCreateDto { };
        var exceptionMessage = "An error occurred.";

        _mapperMock.Setup(mapper => mapper.Map<User>(userCreateDto)).Returns(It.IsAny<User>());
        _logicMock.Setup(userLogic => userLogic.CreateUserAsync(It.IsAny<User>())).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _controller.CreateUserAsync(userCreateDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal(exceptionMessage, statusCodeResult.Value);
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

    [Fact]
    public async Task GetUserByEmailAsync_ReturnsStatusCode500_WhenExceptionIsThrown()
    {
        // Arrange
        var email = "test@example.com";
        var exceptionMessage = "An error occurred while fetching the user.";

        _logicMock.Setup(ul => ul.GetUserByEmailAsync(email)).ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _controller.GetUserByEmailAsync(email);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
    }
}

