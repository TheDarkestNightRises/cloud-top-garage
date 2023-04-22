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

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync()
    {
        return await _context.Garages.Include(g => g.Cars).Include(g => g.Owner).ToListAsync();
    }

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageQuery garageQuery)
    {
        var query = _context.Garages.Include(g => g.Cars).Include(g => g.Owner).AsQueryable();

        if (!(garageQuery.UserId is null))
        {
            query = query.Where(g => g.Owner.Id == garageQuery.UserId);
        }

        return await query.ToListAsync();
    }

    public async Task<Garage> GetGarageAsync(int id)
    {
        var garage = await _context.Garages.Include(g => g.Cars).Include(g => g.Owner).Where(g => g.Id == id).FirstOrDefaultAsync();
        return garage;
    }

}