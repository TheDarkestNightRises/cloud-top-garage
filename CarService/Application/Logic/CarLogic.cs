using Application.LogicContracts;
using CarService.Models;
using CarService.Data;
using MassTransit;
using Contracts;

public class CarLogic : ICarLogic
{
    private readonly ICarRepository _carRepository;

    private readonly IGarageRepository _garageRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CarLogic(ICarRepository carRepository, IPublishEndpoint publishEndpoint, IGarageRepository garageRepository)
    {
        _carRepository = carRepository;
        _publishEndpoint = publishEndpoint;
        _garageRepository = garageRepository;
    }

    public async Task<Car> CreateCarAsync(Car car)
    {
        Garage? garage = await _garageRepository.GetGarageAsync(car.Garage.Id);
        if (garage is null)
        {
            throw new Exception($"Car with id {car.Garage.Id} not found");
        }
        Car carCreated = await _carRepository.CreateCarAsync(car);
        await _publishEndpoint.Publish(new CarCreated(carCreated.Id, garage.Id));
        return carCreated;

    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        var cars = await _carRepository.GetAllCarsAsync();
        foreach (Car car in cars)
        {
            Console.WriteLine(car.ToString());
        }
        return cars;
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync(CarQuery carQuery)
    {
        if (carQuery.GarageId is null && carQuery.CarName is null)
        {
            var cars = await _carRepository.GetAllCarsAsync();
        }
        return await _carRepository.GetAllCarsAsync(carQuery);
    }

    public async Task<Car?> GetCarAsync(int id)
    {
        return await _carRepository.GetCarAsync(id);
    }

    public async Task DeleteCarAsync(int id)
    {
        var car = await _carRepository.GetCarAsync(id);
        if (car is null)
        {
            throw new Exception($"Car with id {id} not found");
        }
        await _publishEndpoint.Publish(new CarDeleted(id));
        await _carRepository.DeleteCarAsync(id);
    }

    public async Task<Image> GetCarImageAsync(int id)
    {
        var car = await _carRepository.GetCarAsync(id);
        if (car is null)
        {
            throw new Exception($"Car with id {id} not found");
        }
        return await _carRepository.GetCarImageAsync(id);
    }

    public async Task<Car> UpdateCarAsync(Car carToUpdate)
    {
        Car? carFound = await _carRepository.GetCarAsync(carToUpdate.Id);
        if (carFound == null)
        {
            throw new ArgumentException($"There is no car with the id: {carToUpdate.Id}");
        }
        Garage? garageFound = await _garageRepository.GetGarageAsync(carToUpdate.Garage.Id);
        if (garageFound == null)
        {
            throw new ArgumentException($"There is no garage with the id: {carToUpdate.Garage.Id}");
        }
        carFound.Garage = garageFound;
        await _publishEndpoint.Publish(new CarUpdated(carToUpdate));
        await _carRepository.UpdateCarAsync(carFound);
        return carFound;
    }

    public async Task<Image> CreateCarImage(Image carImage, int id)
    {
        var car = await _carRepository.GetCarAsync(id);
        if (car is null)
        {
            throw new Exception($"Car with id {id} not found");
        }
        var created = await _carRepository.CreateCarImageAsync(carImage);
        await _carRepository.UpdateCarWithImageAsync(created, id);
        return created;
    }
}