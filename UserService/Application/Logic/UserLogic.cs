namespace UserService.Logic;
using System.Collections.Generic;
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

    public async Task<User> CreateUser(User user)
    {
        var userFound = await _userRepository.GetUserByEmail(user.Email);
        if (userFound != null)
        {
            throw new ArgumentException("Email already exists!");
        }
        user.Role = "User";
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

    public async Task<User> UpdateUser(User userToUpdate)
    {
        User? userFound = await _userRepository.GetUserByIdAsync(userToUpdate.Id);
        if (userFound == null)
        {
            throw new ArgumentException($"There is no user with the id: {userToUpdate.Id}");
        }
        userFound.Email = userToUpdate.Email;
        userFound.Password = userToUpdate.Password;
        userFound.Name = userToUpdate.Name;
        await _userRepository.UpdateUserAsync(userFound);
        return userFound;
    }
}