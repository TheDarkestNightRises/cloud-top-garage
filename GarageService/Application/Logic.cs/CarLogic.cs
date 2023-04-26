
using GarageService.Application.LogicContracts;
using GarageService.Models;
using GarageService.Data;

public class CarLogic : ICarLogic
{
    private readonly ICarRepository _carRepository;
    private readonly IGarageRepository _garageRepository;


    public CarLogic(ICarRepository carRepository, IGarageRepository garageRepository)
    {
        _carRepository = carRepository;
        _garageRepository = garageRepository;
    }

    public async Task CreateCarAsync(int carId, int garageId)
    {

        Garage? garage = await _garageRepository.GetGarageAsync(garageId);
         if(garage == null)
        {
            throw new Exception($"Garage with id {garageId} doesn't exists");
        }
        Car? carExists = await _carRepository.GetCarByIdAsync(carId);
        if(carExists != null)
        {
            throw new Exception($"Car with id {carId} already exists");
        }
        Car car = new Car {
            Id = carId
        };
        garage?.Cars.Add(car);
        await _garageRepository.UpdateGarageAsync(garage); 
    }

    public async Task DeleteCarAsync(int id)
    {
        var car = _carRepository.GetCarByIdAsync(id);

        if (car == null)
        {
            throw new Exception($"Car with id {id} not found");
        }
        await _carRepository.DeleteCarAsync(id);
    }

    public async Task<Car?> GetCarByIdAsync(int carId)
    {
        return await _carRepository.GetCarByIdAsync(carId);
    }

    public async Task UpdateCarAsync(Car car)
    {
        await _carRepository.UpdateCarAsync(car);
    }
}