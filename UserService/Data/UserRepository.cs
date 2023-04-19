namespace UserService.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> UpdateUserAsync(User updatedUser)
    {
        _context.Users.Update( updatedUser);
        await _context.SaveChangesAsync();
        return updatedUser;

    }

}
