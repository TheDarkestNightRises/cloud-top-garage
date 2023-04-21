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

    public async Task DeleteCarAsync(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }

    public async Task<Car?> GetCarByIdAsync(int carId)
    {
        return await _context.Cars.FindAsync(carId);
    }
}