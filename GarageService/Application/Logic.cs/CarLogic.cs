
using GarageService.Application.LogicContracts;
using GarageService.Data;
using GarageService.Models;

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
        var garage = await _garageRepository.GetGarageAsync(garageId);
        Console.WriteLine($"--> Initial garage {garage}");
        if (garage == null)
        {
            throw new ArgumentException("Invalid garage ID");
        }

        if (garage.Cars.Any(c => c.Id == carId))
        {
            throw new ArgumentException("Car already exists in the garage");
        }

        var car = new Car
        {
            Id = carId
        };
        await _carRepository.CreateCarAsync(car);
        garage.Cars.Add(car);
        Console.WriteLine($"---> It was modified {garage}");
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

    public async Task UpdateCarAsync(int carId, int garageId, int currentGarageId)
    {
        Garage? currentGarage = await _garageRepository.GetGarageAsync(currentGarageId);
        Garage? garage = await _garageRepository.GetGarageAsync(garageId);
        if (garage == null)
        {
            throw new ArgumentException("Invalid garage ID");
        }
        Car? car = await _carRepository.GetCarByIdAsync(carId);
        if (car == null)
        {
            throw new Exception($"Car with id {carId} not found");
        }
        currentGarage?.Cars.Remove(car);
        garage.Cars.Add(car);
        await _garageRepository.UpdateGarageAsync(garage);
    }
}
