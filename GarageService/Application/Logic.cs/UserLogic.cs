using GarageService.Application.LogicContracts;
using GarageService.Data;
using GarageService.Models;

public class UserLogic : IUserLogic
{
    private readonly IUserRepository _userRepository;
    public UserLogic(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUserAsync(int userId)
    {
        var user = new User
        {
            Id = userId
        };
        await _userRepository.CreateUserAsync(user);
    }
}
