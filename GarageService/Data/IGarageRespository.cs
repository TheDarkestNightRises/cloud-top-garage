using GarageService.Models;

namespace CarService.Data;

public interface ICarRepository
{
    Task<IEnumerable<Car>> GetAllGaragesAsync();
}