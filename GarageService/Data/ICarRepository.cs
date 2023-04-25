using GarageService.Models;

namespace GarageService.Data;

public interface ICarRepository
{
    Task CreateCarAsync(Car car);
    Task DeleteCarAsync(int id);
    Task<Car?> GetCarByIdAsync(int carId);
}
