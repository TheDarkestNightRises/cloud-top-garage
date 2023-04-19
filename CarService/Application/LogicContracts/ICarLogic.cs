
using CarService.Models;

namespace Application.LogicContracts;

public interface ICarLogic
{
    public Task<IEnumerable<Car>> GetAllCarsAsync();

    public Task<IEnumerable<Car>> GetAllCarsAsync(CarQuery carQuery);

    public Task<IEnumerable<Car>> DeleteCarAsync(int Id);
}