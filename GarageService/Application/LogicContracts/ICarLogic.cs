using GarageService.Models;

namespace GarageService.Application.LogicContracts;

public interface ICarLogic
{
    Task DeleteCarAsync(int id);
    Task<Car?> GetCarByIdAsync(int carId);

}