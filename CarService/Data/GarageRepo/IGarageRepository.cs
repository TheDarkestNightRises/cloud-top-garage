using CarService.Models;

namespace CarService.Data;

public interface IGarageRepository
{
    Task<Garage?> GetGarageAsync(int id);
}