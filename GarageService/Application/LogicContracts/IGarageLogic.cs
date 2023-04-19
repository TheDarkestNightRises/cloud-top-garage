using GarageService.Models;

namespace Application.LogicContracts;

public interface IGarageLogic
{
    public Task<IEnumerable<Garage>> GetAllGaragesAsync();
}