using CarService.Models;

namespace CarService.Data;

public interface ICarRepository
{
    Task<Car> GetCarAsync(int id);
    Task<IEnumerable<Car>> GetAllCarsAsync();
    Task<Car> AddCarAsync(Car car);
    Task<Car> UpdateCarAsync(Car car);
    Task<Car> DeleteCarAsync(int id);

    Task<IEnumerable<Car>> GetAllCarsAsync(CarQuery carQuery);
    Task<Image> GetCarImageAsync(int id);
}