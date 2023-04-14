using UserService.Models;

namespace UserService.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();    
    }
}