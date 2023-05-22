using Contracts;
using GarageService.Models;
using MassTransit;

namespace GarageService.Application.LogicContracts;

public interface ICarLogic
{
    Task CreateCarAsync(int carId, int garageId);
    Task DeleteCarAsync(int id);
    Task<Car?> GetCarByIdAsync(int carId);
    Task UpdateCarAsync(int carId, int garageId, int currentGarageId);
}
