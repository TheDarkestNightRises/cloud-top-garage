using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Data;
using EnvironmentService.Models;

namespace EnvironmentService.Application.Logic;

public class StatLogic : IStatLogic
{
    private readonly IStatRepository _statRepository;

    public StatLogic(IStatRepository statRepository)
    {
        _statRepository = statRepository;
    }

    public async Task<IEnumerable<Stat>> GetAllStatsAsync()
    {
        return await _statRepository.GetAllStatsAsync();
    }

    public async Task<IEnumerable<Stat>> GetAllStatsAsync(StatQuery statQuery)
    {
        if (statQuery.GarageId == 0)
        {
            return await _statRepository.GetAllStatsAsync();
        }
        return await _statRepository.GetAllStatsAsync(statQuery);
    }
    public async Task<Stat?> GetLastestStatAsync(int garageId)
    {
        Stat? stat = await _statRepository.GetLastestStatAsync(garageId);
        return stat;
    }
}
