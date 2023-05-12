using EnvironmentService.Models;

namespace EnvironmentService.Application.LogicContracts;

public interface IStatLogic
{
    Task<IEnumerable<Stat>> GetAllStatsAsync();
}