using EnvironmentService.Models;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentService.Data;


public class IndoorEnvironmentRepository : IIndoorEnvironmentRepository
{
    private readonly AppDbContext _context;

    public IndoorEnvironmentRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<IndoorEnvironment>> GetAllIndoorEnvironmentsAsync()
    {
        return await _context.IndoorEnvironments.Include(indoorEnvironment => indoorEnvironment.Garage).ToListAsync();
    }

    public async Task<IndoorEnvironment?> GetIndoorEnvironmentByMacAdress(int macAddress)
    {
        return await _context.IndoorEnvironments.FirstOrDefaultAsync(i => i.MacAddress == macAddress);
    }
}
