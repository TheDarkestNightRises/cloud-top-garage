
using Application.LogicContracts;
using Carservice.Models;
using CarService.Data;

public class CarLogic : ICarLogic
{
    private readonly ICarRepository _repository;

    public CarLogic(ICarRepository carRepository)
    {
        _repository = carRepository;
    }
    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        return await _repository.GetAllCarsAsync();

    }
}