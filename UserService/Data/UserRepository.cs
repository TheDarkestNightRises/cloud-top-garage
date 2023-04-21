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

    public async Task CreateUser(User user)
    {
       _context.Users.Add(user); // Add the user to the database context 
       await _context.SaveChangesAsync(); // Save the changes to the database

    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> getUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> UpdateUserAsync(User userFound)
    {
        _context.Users.Update(userFound);
        await _context.SaveChangesAsync();
        return userFound;

    }

}
