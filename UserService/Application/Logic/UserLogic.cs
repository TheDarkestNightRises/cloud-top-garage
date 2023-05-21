namespace UserService.Logic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;

public class UserLogic : IUserLogic
{
    private readonly IUserRepository _userRepository;

    public UserLogic(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var userFound = await _userRepository.GetUserByEmail(user.Email);
        if (userFound != null)
        {
            throw new ArgumentException("Email already exists!");
        }
        user.Role = "User";
        Validator.ValidateObject(user, new ValidationContext(user), validateAllProperties: true);
        return await _userRepository.CreateUserAsync(user);
    }


    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> LoginUserAsync(string email, string password)
    {
        User? userFound = await _userRepository.GetUserByEmail(email);

        if (userFound == null)
        {
            throw new ArgumentException("Email doesn't exist!");
        }
        if (!userFound.Password.Equals(password))
        {
            throw new ArgumentException("Wrong password!");
        }
        return userFound;

    }

    public async Task<User> UpdateUserAsync(User userToUpdate)
    {
        User? existingUser = await _userRepository.GetUserByIdAsync(userToUpdate.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"There is no user with the id: {userToUpdate.Id}");
        }

        existingUser = new User
        {

            Id = existingUser.Id,
            Email = userToUpdate.Email,
            Role = existingUser.Role,
            Password = userToUpdate.Password,
            Name = userToUpdate.Name,
            Age = userToUpdate.Age,
            Phone = userToUpdate.Phone
        };
        Validator.ValidateObject(existingUser, new ValidationContext(existingUser), validateAllProperties: true);

        await _userRepository.UpdateUserAsync(existingUser);

        return existingUser;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }

}