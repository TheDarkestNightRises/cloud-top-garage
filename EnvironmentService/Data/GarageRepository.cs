using EnvironmentService.Models;

namespace EnvironmentService.Data;

public class GarageRepository : IGarageRepository
{
    private readonly AppDbContext _context;
    public GarageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Garage> CreateGarageAsync(Garage garage)
    {
        await _context.Garages.AddAsync(garage);
        await _context.SaveChangesAsync();
        return garage;
    }

    public async Task DeleteGarageAsync(Garage garage)
    {
        _context.Garages.Remove(garage);
        await _context.SaveChangesAsync();
    }

    public async Task<Garage> GetGarageByIdAsync(int garageId)
    {
        return await _context.Garages.FindAsync(garageId);
    }
}