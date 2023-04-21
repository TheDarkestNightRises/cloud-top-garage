using GarageService.Models;
using Microsoft.EntityFrameworkCore;

namespace GarageService.Data;

public class CarRepository : ICarRepository
{
    private AppDbContext _context;

    public CarRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task DeleteCarAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Car> GetCarByIdAsync(int carId)
    {
        throw new NotImplementedException();
    }
}