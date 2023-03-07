using CarService.Models;
using Microsoft.EntityFrameworkCore;

namespace CarService.Data;

public class CarRepository : ICarRepository
{
    private AppDbContext _context;

    public CarRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Car> AddCarAsync(Car car)
    {
        throw new NotImplementedException();
    }

    public Task<Car> DeleteCarAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        return await _context.Cars.ToListAsync();
    }

    public Task<Car> GetCarAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Car> UpdateCarAsync(Car car)
    {
        throw new NotImplementedException();
    }
}