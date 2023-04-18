using UserService.Models;

namespace UserService.Data;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> UpdateUserPasswordAsync(User userToUpdate);
}
