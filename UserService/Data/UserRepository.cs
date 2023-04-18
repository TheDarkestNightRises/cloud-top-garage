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

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> UpdateUserPasswordAsync(User userToUpdate)
    {
        _context.Users.Update(userToUpdate);
        await _context.SaveChangesAsync();
        return userToUpdate;

    }

}
