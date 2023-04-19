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

    public async Task<Car> DeleteCarAsync(int id)
    {
        var carToDelete = await  _context.Cars.FindAsync(id);
        if (carToDelete != null)
        {
            _context.Cars.Remove(carToDelete);
            await _context.SaveChangesAsync();
        }
        return carToDelete;
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        return await _context.Cars.Include(c => c.Garage).ToListAsync();
    }

    public Task<Car> GetCarAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Car> UpdateCarAsync(Car car)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync(CarQuery carQuery)
    {
        var query = _context.Cars.AsQueryable();

        if (!(carQuery.GarageId is null))
        {
            query = query.Where(c => c.Garage.Id == carQuery.GarageId);
        }

        if (!string.IsNullOrEmpty(carQuery.CarName))
        {
            query = query.Where(c => c.Name.Contains(carQuery.CarName, StringComparison.OrdinalIgnoreCase));
        }

        return await query.ToListAsync();
    }
}