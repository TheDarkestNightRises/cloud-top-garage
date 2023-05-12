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
}