using EnvironmentService.Models;

namespace EnvironmentService.Data;
public interface IStatRepository
{
    Task<Stat> AddStatAsync(Stat stat);
    Task<IEnumerable<Stat>> GetAllStatsAsync();
    Task<IEnumerable<Stat>> GetAllStatsAsync(StatQuery statQuery);
    Task<Stat?> GetLastestStatAsync(int garageId);
}
