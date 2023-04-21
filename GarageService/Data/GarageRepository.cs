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
        return await _context.Garages.Include(g => g.Cars).ToListAsync();
    }

}