using UserService.Models;

namespace UserService.Data;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> getUserByEmail(string email);
    Task<User?> GetUserByIdAsync(int id);
    Task<User> UpdateUserAsync(User userFound);
}
