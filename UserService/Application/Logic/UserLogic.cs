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

    public async Task<User> UpdateUserPassword(UserUpdateDto userUpdateDto)
    {
        User? userFound = await _userRepository.GetUserByEmailAsync(userUpdateDto.Email);
        if(userFound == null) 
        {
           throw new Exception($"There is no user with this email: {userUpdateDto.Email} ");
        }
        userFound.Password = userUpdateDto.Password;
        await _userRepository.UpdateUserAsync(userFound);
        return userFound;
    }
}