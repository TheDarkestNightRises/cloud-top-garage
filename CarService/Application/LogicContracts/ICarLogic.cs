
using CarService.Models;

namespace Application.LogicContracts;

public interface ICarLogic
{
    public Task<Car> CreateAsync(Car car);
    public Task<IEnumerable<Car>> GetAllCarsAsync();
    public Task<IEnumerable<Car>> GetAllCarsAsync(CarQuery carQuery);
}