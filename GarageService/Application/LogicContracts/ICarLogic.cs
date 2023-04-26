using Contracts;
using GarageService.Models;
using MassTransit;

namespace GarageService.Application.LogicContracts;

public interface ICarLogic
{
    Task CreateCarAsync(Car car);
    Task DeleteCarAsync(int id);
    Task<Car?> GetCarByIdAsync(int carId);
    Task UpdateCarAsync(Car car);
}