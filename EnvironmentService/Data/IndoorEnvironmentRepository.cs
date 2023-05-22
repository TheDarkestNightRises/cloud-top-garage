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
        return await _context.IndoorEnvironments.Include(indoorEnvironment => indoorEnvironment.Garage).Include(indoorEnvironment => indoorEnvironment.IndoorEnvironmentSettings).ToListAsync();
    }

    public async Task<IndoorEnvironment?> GetIndoorEnvironmentByIdAsync(int id)
    {
        return await _context.IndoorEnvironments.Include(indoorEnvironment => indoorEnvironment.Garage).Include(indoorEnvironment => indoorEnvironment.IndoorEnvironmentSettings).FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task<IndoorEnvironment?> GetIndoorEnvironmentByMacAdress(int macAddress)
    {
        return await _context.IndoorEnvironments.FirstOrDefaultAsync(i => i.IndoorEnvironmentSettings.MacAddress == macAddress);
    }

    public async Task<IndoorEnvironment> UpdateIndoorEnvironment(IndoorEnvironment indoorEnvironment)
    {
        _context.IndoorEnvironments.Update(indoorEnvironment);
        await _context.SaveChangesAsync();
        return indoorEnvironment;
    }
}
