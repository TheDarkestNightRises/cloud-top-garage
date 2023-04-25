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

    public async Task<Car> AddCarAsync(Car car)
    {
        var garage = await _context.Garages.FindAsync(car.Garage.Id);
        car.Garage = garage;
        await _context.AddAsync(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task DeleteCarAsync(int id)
    {
        var carToDelete = await _context.Cars.FindAsync(id);
        if (carToDelete != null)
        {
            _context.Cars.Remove(carToDelete);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        return await _context.Cars.Include(c => c.Garage)
                                    .ToListAsync();
    }

    public async Task<Car?> GetCarAsync(int id)
    {
        var car = await _context.Cars.Include(c => c.Garage).FirstOrDefaultAsync(c => c.Id == id);
        return car;
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

    public async Task<Image> GetCarImageAsync(int id)
    {
        var car = await _context.Cars
        .Include(c => c.Image)
        .Where(c => c.Id == id)
        .SingleOrDefaultAsync();
        return car.Image;
    }

    public async Task<Image> CreateCarImageAsync(Image carImage, int id)
    {
        await _context.Images.AddAsync(carImage);
        await _context.SaveChangesAsync();
        return carImage;
    }
}