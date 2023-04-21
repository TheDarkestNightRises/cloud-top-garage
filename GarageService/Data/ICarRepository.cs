using GarageService.Models;

namespace GarageService.Data;

public interface ICarRepository
{
    Task DeleteCarAsync(int id);
    Task<Car> GetCarByIdAsync(int carId);
}
