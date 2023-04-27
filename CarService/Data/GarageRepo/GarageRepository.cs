using CarService.Models;
using Microsoft.EntityFrameworkCore;

namespace CarService.Data;

public class GarageRepository : IGarageRepository
{
    private AppDbContext _context;

    public GarageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Garage?> GetGarageAsync(int id)
    {
        return await _context.Garages.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task DeleteGarageAsync(int id)
    {
        var garageToDelete = _context.Garages.Find(id);
        _context.Garages.Remove(garageToDelete);
        await _context.SaveChangesAsync();
    }
}