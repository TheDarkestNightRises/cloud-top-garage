using EnvironmentService.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentService.Data;
public class StatRepository : IStatRepository
{
    private readonly AppDbContext _context;

    public StatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Stat> AddStatAsync(Stat stat)
    {
        await _context.Stats.AddAsync(stat);
        await _context.SaveChangesAsync();
        return stat;
    }

    public async Task<IEnumerable<Stat>> GetAllStatsAsync()
    {
        return await _context.Stats.Include(stat => stat.IndoorEnvironment).ToListAsync();
    }

    public async Task<IEnumerable<Stat>> GetAllStatsAsync(StatQuery statQuery)
    {
        var query = _context.Stats.Include(stat => stat.IndoorEnvironment).AsQueryable();

        if (statQuery.GarageId != 0)
        {
            query = query.Where(s => s.IndoorEnvironment.Garage.Id == statQuery.GarageId);
        }

        var stats = await query.ToListAsync();
        return stats;
    }
    public async Task<Stat?> GetLastestStatAsync(int garageId)
    {
        var mostRecentStat = await _context.Stats
               .Where(stat => stat.IndoorEnvironment.Garage.Id == garageId)
               .OrderByDescending(stat => stat.Time)
               .FirstOrDefaultAsync();

        return mostRecentStat;
    }
}
