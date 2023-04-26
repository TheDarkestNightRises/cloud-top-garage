
using GarageService.Application.LogicContracts;
using GarageService.Models;
using GarageService.Data;

public class CarLogic : ICarLogic
{
    private readonly ICarRepository _repository;

    public CarLogic(ICarRepository carRepository)
    {
        _repository = carRepository;
    }

    public async Task CreateCarAsync(Car car)
    {
       await _repository.CreateCarAsync(car);
    }

    public async Task DeleteCarAsync(int id)
    {
        var car = _repository.GetCarByIdAsync(id);

        if (car == null)
        {
            throw new Exception($"Car with id {id} not found");
        }
        await _repository.DeleteCarAsync(id);
    }

    public async Task<Car?> GetCarByIdAsync(int carId)
    {
        return await _repository.GetCarByIdAsync(carId);
    }

    public async Task UpdateCarAsync(Car car)
    {
       await _repository.UpdateCarAsync(car);
    }
}