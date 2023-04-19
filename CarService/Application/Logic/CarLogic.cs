
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
        if (carQuery.GarageId is null && carQuery.CarName is null) return await _repository.GetAllCarsAsync();
        return await _repository.GetAllCarsAsync(carQuery);
    }

    public async Task<Car> GetCarAsync(int id)
    {
        return await _repository.GetCarAsync(id);
    }
}