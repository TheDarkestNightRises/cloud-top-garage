using GarageService.Models;

namespace GarageService.Application.LogicContracts;

public interface IGarageLogic
{
    public Task<IEnumerable<Garage>> GetAllGaragesAsync();
}