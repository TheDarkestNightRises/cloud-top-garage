using EnvironmentService.Models;

namespace EnvironmentService.Application.LogicContracts;

public interface IGarageLogic
{
    Task<Garage> CreateGarageAsync(int garageId);
    Task DeleteGarageAsync(int garageId);
}
