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

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> LoginUserAsync(string email, string password)
    {
        User? userFound = await _userRepository.getUserByEmail(email);

        if (userFound == null)
        {
            throw new Exception("Email doesn't exist!");
        }
        if (!userFound.Password.Equals(password))
        {
            throw new Exception("Wrong password!");
        }
        return userFound;

    }

    public async Task<User> UpdateUser(User userToUpdate)
    {
        User? userFound = await _userRepository.GetUserByIdAsync(userToUpdate.Id);
        if (userFound == null)
        {
            throw new Exception($"There is no user with the id: {userToUpdate.Id}");
        }
        userFound.Email = userToUpdate.Email;
        userFound.Password = userToUpdate.Password;
        await _userRepository.UpdateUserAsync(userFound);
        return userFound;
    }
}