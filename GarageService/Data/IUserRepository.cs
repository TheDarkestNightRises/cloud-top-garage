using GarageService.Models;

namespace GarageService.Data;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int UserId);
    Task CreateUserAsync(User user);
}
