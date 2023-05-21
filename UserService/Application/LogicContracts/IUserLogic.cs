using UserService.Dtos;
using UserService.Models;

namespace UserService.Logic;
public interface IUserLogic
{
    Task<User> CreateUserAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> LoginUserAsync(string email, string password);
    Task<User> UpdateUserAsync(User userToUpdate);

}
