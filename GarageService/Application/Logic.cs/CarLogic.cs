
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

    public Task DeleteCarAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Car> GetCarByIdAsync(int carId)
    {
        throw new NotImplementedException();
    }
}