using EnvironmentService.Models;

namespace EnvironmentService.Application.LogicContracts;

public interface IStatLogic
{
    Task<IEnumerable<Stat>> GetAllStatsAsync(StatQuery statQuery);
    Task<IEnumerable<Stat>> GetAllStatsAsync();
}
