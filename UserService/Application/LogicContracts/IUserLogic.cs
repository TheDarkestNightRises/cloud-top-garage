using UserService.Models;

namespace UserService.Logic;
public interface IUserLogic
{
    Task<IEnumerable<User>> GetAllUsersAsync();
}
