using GarageService.Models;
using Microsoft.EntityFrameworkCore;

namespace GarageService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }

    public DbSet<Car> Cars { get; set; }

    public DbSet<Garage> Garages { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Location> Locations { get; set; }

}
