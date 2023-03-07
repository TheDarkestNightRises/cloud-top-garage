using CarService.Models;
using Microsoft.EntityFrameworkCore;

namespace CarService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }

    public DbSet<Car> Cars { get; set; }
}