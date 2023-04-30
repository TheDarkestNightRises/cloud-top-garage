using GarageService.Models;

namespace GarageService.Data;

public interface IGarageRepository
{
    Task<IEnumerable<Garage>> GetAllGaragesAsync();
    Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageQuery garageQuery);
    Task<Garage?> GetGarageAsync(int id);
    Task DeleteGarageAsync(int id);
    Task UpdateGarageAsync(Garage garage);
    Task<Garage> CreateGarageAsync(Garage garage);
}