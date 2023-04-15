using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("userdb");
    }

    public DbSet<User> Users { get; set; }

}