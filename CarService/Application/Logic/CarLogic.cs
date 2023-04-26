
using Application.LogicContracts;
using CarService.Models;
using CarService.Data;
using MassTransit;
using Contracts;

public class CarLogic : ICarLogic
{
    private readonly ICarRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;

    public CarLogic(ICarRepository carRepository, IPublishEndpoint publishEndpoint, IBus bus)
    {
        _repository = carRepository;
        _publishEndpoint = publishEndpoint;
        _bus = bus;
    }

    public async Task<Car> CreateAsync(Car car)
    {
        var response = await _bus.Request<CheckGarage, GarageChecked>(new CheckGarage(car.Garage.Id));
        if (response.Message.IsGarageValid)
        {
            return await _repository.AddCarAsync(car);
        }
        else
        {
            throw new Exception("Garage does not exist");
        }
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        var cars = await _repository.GetAllCarsAsync();
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
            var cars = await _repository.GetAllCarsAsync();
        }
        return await _repository.GetAllCarsAsync(carQuery);
    }

    public async Task<Car?> GetCarAsync(int id)
    {
        return await _repository.GetCarAsync(id);
    }

    public async Task DeleteCarAsync(int id)
    {
        var car = await _repository.GetCarAsync(id);
        if (car is null)
        {
            throw new Exception($"Car with id {id} not found");
        }
        await _publishEndpoint.Publish(new CarDeleted(id));
        await _repository.DeleteCarAsync(id);
    }

    public async Task<Image> GetCarImageAsync(int id)
    {
        var car = await _repository.GetCarAsync(id);
        if (car is null)
        {
            throw new Exception($"Car with id {id} not found");
        }
        return await _repository.GetCarImageAsync(id);
    }

    public async Task<Image> CreateCarImage(Image carImage, int id)
    {
        var car = await _repository.GetCarAsync(id);
        if (car is null)
        {
            throw new Exception($"Car with id {id} not found");
        }
        var created = await _repository.CreateCarImageAsync(carImage);
        await _repository.UpdateCarWithImageAsync(created, id);
        return created;
    }
}