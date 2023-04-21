using UserService.Models;

namespace UserService.Data;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserByIdAsync(int id);
    Task<User> UpdateUserAsync(User userFound);
    Task<User> CreateUserAsync(User user);


}
