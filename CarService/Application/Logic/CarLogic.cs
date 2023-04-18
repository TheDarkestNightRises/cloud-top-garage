
using Application.LogicContracts;
using CarService.Models;
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

    public async Task<IEnumerable<Car>> GetAllCarsAsync(CarQuery carQuery)
    {
        return await _repository.GetAllCarsAsync(carQuery);
    }
}