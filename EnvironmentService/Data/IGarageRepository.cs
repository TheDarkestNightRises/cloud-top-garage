using EnvironmentService.Models;

namespace EnvironmentService.Data;

public interface IGarageRepository
{
    Task<Garage> CreateGarageAsync(Garage garage);
    Task DeleteGarageAsync(Garage garage);
    Task<Garage> GetGarageByIdAsync(int garageId);
}
