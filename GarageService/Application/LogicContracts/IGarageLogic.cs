using GarageService.Models;

namespace GarageService.Application.LogicContracts;

public interface IGarageLogic
{
    public Task<Garage> CreateGarageAsync(Garage garage);
    public Task<IEnumerable<Garage>> GetAllGaragesAsync();
    public Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageQuery garageQuery);
    public Task<Garage?> GetGarageAsync(int id);

    public Task DeleteGarageAsync(int id);
}