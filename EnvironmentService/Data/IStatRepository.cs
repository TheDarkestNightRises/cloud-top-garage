using EnvironmentService.Models;

namespace EnvironmentService.Data;
public interface IStatRepository
{
    Task<Stat> AddStatAsync(Stat stat);
    Task<IEnumerable<Stat>> GetAllStatsAsync();
}