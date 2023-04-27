using GarageService.Models;
using Microsoft.EntityFrameworkCore;

namespace GarageService.Data;

public class GarageRepository : IGarageRepository
{
    private AppDbContext _context;

    public GarageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task DeleteGarageAsync(int id)
    {
        var garageToDelete = _context.Garages.Find(id);
        _context.Garages.Remove(garageToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync()
    {

        var garages = await _context.Garages.Include(g => g.Cars)
                                            .Include(g => g.User)
                                            .Include(g => g.Location)
                                            .ToListAsync();
        return garages;
    }

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageQuery garageQuery)
    {
        var query = _context.Garages.Include(g => g.Cars)
                                    .Include(g => g.User)
                                    .Include(g => g.Location)
                                    .AsQueryable();

        if (!(garageQuery.UserId is null))
        {
            query = query.Where(g => g.User.Id == garageQuery.UserId);
        }
        var garages = await query.ToListAsync();

        return garages;
    }

    public async Task<Garage?> GetGarageAsync(int id)
    {
        var garage = await _context.Garages.Include(g => g.Cars)
                                            .Include(g => g.User)
                                            .Include(g => g.Location)
                                            .FirstOrDefaultAsync(g => g.Id == id);
        return garage;
    }

    public async Task UpdateGarageAsync(Garage garage)
    {
        _context.Entry(garage).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}