
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
}