using UserService.Dtos;
using UserService.Models;

namespace UserService.Logic;
public interface IUserLogic
{
    Task<User> CreateUser(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> LoginUserAsync(string email, string password);
    Task<User> UpdateUser(User userToUpdate);
    
}
