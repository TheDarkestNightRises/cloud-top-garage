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
        modelBuilder.Entity<User>()
        .Property(u => u.Name)
        .HasMaxLength(15)
        .IsRequired();


        modelBuilder.Entity<User>()
        .Property(u => u.Email)
        .HasMaxLength(20)
        .IsRequired();

        modelBuilder.Entity<User>()
        .Property(u => u.Password)
        .HasMaxLength(15)
        .IsRequired();
    }

    public DbSet<User> Users { get; set; }


}
