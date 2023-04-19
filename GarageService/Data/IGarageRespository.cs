using GarageService.Models;

namespace GarageService.Data;

public interface IGarageRepository
{
    Task<IEnumerable<Garage>> GetAllGaragesAsync();
}