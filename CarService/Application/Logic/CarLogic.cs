using System.ComponentModel.DataAnnotations;
using Application.LogicContracts;
using CarService.Data;
using CarService.Models;
using Contracts;
using MassTransit;

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
        ValidateCar(car);
        ValidateEngine(car.Engine);
        Garage? garage = await _garageRepository.GetGarageAsync(car.Garage.Id);
        if (garage is null)
        {
            throw new ArgumentException($"Garage with id {car.Garage.Id} not found");
        }

        car.Garage = garage;
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
            throw new ArgumentException($"Car with id {id} not found");
        }
        await _publishEndpoint.Publish(new CarDeleted(id));
        await _carRepository.DeleteCarAsync(id);
    }

    public async Task<Image> GetCarImageAsync(int id)
    {
        var car = await _carRepository.GetCarAsync(id);
        if (car is null)
        {
            throw new ArgumentException($"Car with id {id} not found");
        }
        return await _carRepository.GetCarImageAsync(id);
    }

    public async Task<Car> UpdateCarAsync(Car carToUpdate)
    {
        Car? carFound = await _carRepository.GetCarAsync(carToUpdate.Id);
        if (carFound == null)
        {
            throw new ArgumentException($"Car with id {carToUpdate.Id} not found");
        }
        Garage? garageFound = await _garageRepository.GetGarageAsync(carToUpdate.Garage.Id);
        if (garageFound == null)
        {
            throw new ArgumentException($"Garage with id {carToUpdate.Garage.Id} not found");
        }
        carFound.Garage = garageFound;
        await _publishEndpoint.Publish(new CarMoved(carFound.Id, garageFound.Id, carFound.Garage.Id));
        await _carRepository.UpdateCarAsync(carFound);
        return carFound;
    }

    public async Task<Image> CreateCarImage(Image carImage, int id)
    {
        var car = await _carRepository.GetCarAsync(id);
        if (car is null)
        {
            throw new ArgumentException($"Car with id {id} not found");
        }
        var created = await _carRepository.CreateCarImageAsync(carImage);
        await _carRepository.UpdateCarWithImageAsync(created, id);
        return created;
    }

    private void ValidateEngine(Engine engine) 
    {
        Validator.ValidateObject(engine, new ValidationContext(engine), validateAllProperties: true);
    }

    private void ValidateCar(Car car) {
        if (car.Year > DateTime.Now.Year)
        {
            throw new ValidationException("Year cannot be bigger than current year");
        }
        Validator.ValidateObject(car, new ValidationContext(car), validateAllProperties: true);
    }
}
