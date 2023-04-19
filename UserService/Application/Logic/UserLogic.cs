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

    public async Task<User> UpdateUser(User userUpdate)
    {
        User? userFound = await _userRepository.GetUserByIdAsync(userUpdate.Id);
        if(userFound == null) 
        {
           throw new Exception($"There is no user with the id: {userUpdate.Id}");
        }
        userFound.Email = userUpdate.Email;
        userFound.Password = userUpdate.Password;
        var updatedUser = userFound;
        await _userRepository.UpdateUserAsync(updatedUser);
        return updatedUser;
    }
}