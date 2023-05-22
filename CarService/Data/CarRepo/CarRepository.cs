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

    public async Task<Car> CreateCarAsync(Car car)
    {
        await _context.AddAsync(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task DeleteCarAsync(int id)
    {
        var carToDelete = await _context.Cars.FindAsync(id);
        _context.Cars.Remove(carToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        return await _context.Cars.Include(c => c.Garage)
                                    .Include(c => c.Engine)
                                    .ToListAsync();
    }

    public async Task<Car?> GetCarAsync(int id)
    {
        var car = await _context.Cars.Include(c => c.Garage)
                                        .Include(c => c.Engine)
                                        .FirstOrDefaultAsync(c => c.Id == id);
        return car;
    }

    public async Task<Car> UpdateCarAsync(Car car)
    {
        _context.Cars.Update(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync(CarQuery carQuery)
    {
        var query = _context.Cars.Include(c => c.Garage)
                                .Include(c => c.Engine)
                                .AsQueryable();

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

    public async Task<Image> GetCarImageAsync(int id)
    {
        var car = await _context.Cars
        .Include(c => c.Image)
        .Include(c => c.Engine)
        .Where(c => c.Id == id)
        .SingleOrDefaultAsync();
        return car.Image;
    }

    public async Task<Image> CreateCarImageAsync(Image carImage)
    {
        await _context.Images.AddAsync(carImage);
        await _context.SaveChangesAsync();
        return carImage;
    }

    public async Task UpdateCarWithImageAsync(Image carImage, int carId)
    {
        var car = await _context.Cars.FindAsync(carId);
        car.Image = carImage;
        await _context.SaveChangesAsync();
    }
}
