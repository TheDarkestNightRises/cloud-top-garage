
using Application.LogicContracts;
using CarService.Models;
using CarService.Data;
using MassTransit;

public class CarLogic : ICarLogic
{
    private readonly ICarRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CarLogic(ICarRepository carRepository, IPublishEndpoint publishEndpoint)
    {
        _repository = carRepository;
        _publishEndpoint = publishEndpoint;
    }

    public Task<Car> CreateAsync(Car car)
    {
        throw new NotImplementedException();
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

    public async Task<Car> GetCarAsync(int id)
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
}