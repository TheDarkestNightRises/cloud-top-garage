using EnvironmentService.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }

    public DbSet<IndoorEnvironment> Environments { get; set; }
}