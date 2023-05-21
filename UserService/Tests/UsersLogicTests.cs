using Moq;
using UserService.Data;
using UserService.Logic;
using UserService.Models;
using Xunit;

public class UserLogicTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserLogic _userLogic;

    public UserLogicTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userLogic = new UserLogic(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_WhenValidUser_ReturnsCreatedUser()
    {
        // Arrange
        var user = new User
        {
            Name = "John Doe",
            Email = "test@example.com",
            Password = "ValidPassword123!",
            Role = "User",
            Age = 25,
            Phone = "1234567890"
            // Set other properties as needed
        };

        // Set up mock behavior
        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(user.Email)).ReturnsAsync((User)null);
        _userRepositoryMock.Setup(repo => repo.CreateUserAsync(user)).ReturnsAsync(user);

        // Act
        var createdUser = await _userLogic.CreateUserAsync(user);

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(user.Name, createdUser.Name);
        Assert.Equal(user.Email, createdUser.Email);
        Assert.Equal(user.Role, createdUser.Role);
        Assert.Equal(user.Age, createdUser.Age);
        Assert.Equal(user.Phone, createdUser.Phone);
    }

    [Fact]
    public async Task CreateUserAsync_WhenIsExistingEmail_ThrowsArgumentException()
    {
        // Arrange
        var user = new User
        {
            Name = "Jane Doe",
            Email = "existing@example.com",
            Password = "ValidPassword123!",
            Role = "User",
            Age = 30,
            Phone = "9876543210"

        };

        // Set up mock behavior
        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(user.Email)).ReturnsAsync(user);

        // Act and Assert
        var exception = await Record.ExceptionAsync(async () => await _userLogic.CreateUserAsync(user));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("Email already exists!", exception.Message);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "ValidPassword123!",
                Role = "User",
                Age = 25,
                Phone = "1234567890"
            },
            new User
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                Password = "StrongPassword456!",
                Role = "User",
                Age = 30,
                Phone = "9876543210"
            }
        };

        _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var result = await _userLogic.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(users.SequenceEqual(result));
    }

    [Fact]
    public async Task GetUserByEmailAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User
        {
            Name = "John Doe",
            Email = email,
            Password = "ValidPassword123!",
            Role = "User",
            Age = 25,
            Phone = "1234567890"
            // Set other properties as needed
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);

        // Act
        var result = await _userLogic.GetUserByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var email = "nonexistent@example.com";

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync((User)null);

        // Act
        var result = await _userLogic.GetUserByEmailAsync(email);

        // Assert
        Assert.Null(result);
    }
    [Fact]
    public async Task LoginUserAsync_WhenValidCredentials_ReturnsUser()
    {
        // Arrange
        var email = "test@example.com";
        var password = "ValidPassword123!";
        var user = new User
        {
            Name = "John Doe",
            Email = email,
            Password = password,
            Role = "User",
            Age = 25,
            Phone = "1234567890"
            // Set other properties as needed
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);

        // Act
        var result = await _userLogic.LoginUserAsync(email, password);

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task LoginUserAsync_WhenInvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "ValidPassword123!";

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync((User)null);

        // Act and Assert
        var exception = await Record.ExceptionAsync(async () => await _userLogic.LoginUserAsync(email, password));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("Email doesn't exist!", exception.Message);
    }

    [Fact]
    public async Task LoginUserAsync_WhenInvalidPassword_ThrowsArgumentException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "InvalidPassword123!";
        var user = new User
        {
            Name = "John Doe",
            Email = email,
            Password = "ValidPassword123!",
            Role = "User",
            Age = 25,
            Phone = "1234567890"
            // Set other properties as needed
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);

        // Act and Assert
        var exception = await Record.ExceptionAsync(async () => await _userLogic.LoginUserAsync(email, password));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("Wrong password!", exception.Message);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenValidUser_ReturnsUpdatedUser()
    {
        // Arrange
        var existingUser = new User
        {
            Id = 1,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "OldPassword123!",
            Role = "User",
            Age = 30,
            Phone = "1234567890"
            // Set other properties as needed
        };

        var updatedUser = new User
        {
            Id = 1,
            Name = "John Smith",
            Email = "john.smith@example.com",
            Password = "NewPassword123!",
            Role = "User",
            Age = 35,
            Phone = "9876543210"
            // Set other properties as needed
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(existingUser.Id)).ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(updatedUser)).ReturnsAsync(updatedUser);
        


        // Act
        var result = await _userLogic.UpdateUserAsync(updatedUser);
        Console.WriteLine(result);
        Console.WriteLine(updatedUser);


        // Assert
        Assert.Equal(updatedUser, result);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenInvalidUserId_ThrowsArgumentException()
    {
        // Arrange
        var userToUpdate = new User
        {
            Id = 2,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Password = "NewPassword123!",
            Role = "User",
            Age = 30,
            Phone = "1234567890"
            // Set other properties as needed
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userToUpdate.Id)).ReturnsAsync((User)null);

        // Act and Assert
        var exception = await Record.ExceptionAsync(async () => await _userLogic.UpdateUserAsync(userToUpdate));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal($"There is no user with the id: {userToUpdate.Id}", exception.Message);
    }


}
