
using CarService.Models;

namespace Application.LogicContracts;

public interface ICarLogic
{
    public Task<Car> CreateAsync(Car car);
    public Task<IEnumerable<Car>> GetAllCarsAsync();
    public Task<IEnumerable<Car>> GetAllCarsAsync(CarQuery carQuery);
    public Task<Car> GetCarAsync(int id);
    public Task DeleteCarAsync(int Id);
    Task<Image> GetCarImageAsync(int id);
}