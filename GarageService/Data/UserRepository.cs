using GarageService.Models;
using Microsoft.EntityFrameworkCore;

namespace GarageService.Data;

public class UserRepository : IUserRepository
{
    private AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public UserRepository()
    {
    }

    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    // public async Task DeleteUserAsync(int id)
    // {
    //     var user = await _context.Users.FindAsync(id);
    //     _context.Users.Remove(user);
    //     await _context.SaveChangesAsync();
    // }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }
}