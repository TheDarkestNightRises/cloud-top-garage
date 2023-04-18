using UserService.Dtos;
using UserService.Models;

namespace UserService.Logic;
public interface IUserLogic
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task UpdateUserPassword(UserUpdateDto userUpdateDto);
}
