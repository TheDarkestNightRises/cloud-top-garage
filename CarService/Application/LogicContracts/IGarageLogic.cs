using CarService.Models;

namespace CarService.Application.LogicContracts;

public interface IGarageLogic
{
    public Task DeleteGarageAsync(int id);

    public Task<Garage?> GetGarageAsync(int id);
}