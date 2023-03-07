
using Carservice.Models;

namespace Application.LogicContracts;

public interface ICarLogic
{
    public Task<IEnumerable<Car>> GetAllCarsAsync();
}