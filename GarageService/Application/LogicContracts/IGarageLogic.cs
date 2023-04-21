using GarageService.Models;

namespace GarageService.Application.LogicContracts;

public interface IGarageLogic
{
    Task DeleteCarAsync(int id);
    public Task<IEnumerable<Garage>> GetAllGaragesAsync();
}