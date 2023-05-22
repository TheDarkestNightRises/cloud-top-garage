using EnvironmentService.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }

    public DbSet<IndoorEnvironment> IndoorEnvironments { get; set; }
    public DbSet<Stat> Stats { get; set; }
    public DbSet<Garage> Garages { get; set; }
}
