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


}
