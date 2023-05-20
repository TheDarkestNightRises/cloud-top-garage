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
        // Get the first garage that matches the given id
        var garageToDelete = _context.Garages.Find(id);
        // Removes the garage from the database
        _context.Garages.Remove(garageToDelete);
        // Save changes
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync()
    {
        // Get all garages along with data about cars, users, and locations
        var garages = await _context.Garages.Include(g => g.Cars)
                                            .Include(g => g.User)
                                            .Include(g => g.Location)
                                            .ToListAsync();

        // Return found garages                                    
        return garages;
    }

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageQuery garageQuery)
    {
        // Get all garages along with data about cars, users, and locations
        var query = _context.Garages.Include(g => g.Cars)
                                    .Include(g => g.User)
                                    .Include(g => g.Location)
                                    .AsQueryable();
        // Checks if there is any input regarding a specific user's id
        // If the input is empty all garages will be retrieved
        // If the input is an user id, then only that user's garages will be retrived
        if (!(garageQuery.UserId is null))
        {
            query = query.Where(g => g.User.Id == garageQuery.UserId);
        }
        // Get a list of all garages
        var garages = await query.ToListAsync();
        // Return garages
        return garages;
    }

    public async Task<Garage?> GetGarageAsync(int id)
    {
        // Get the first garage along with data about cars, users, and locations
        var garage = await _context.Garages.Include(g => g.Cars)
                                            .Include(g => g.User)
                                            .Include(g => g.Location)
                                            .FirstOrDefaultAsync(g => g.Id == id);
        //Return the garage                         
        return garage;
    }

    public async Task UpdateGarageAsync(Garage garage)
    {
        // Modify the state of the garage
        _context.Entry(garage).State = EntityState.Modified;
        // Saves changes
        await _context.SaveChangesAsync();
    }

    public async Task<Garage> CreateGarageAsync(Garage garage)
    {
        // Adding the garage to the database
        await _context.Garages.AddAsync(garage);
        // Saves the changes
        await _context.SaveChangesAsync();
        // Return the garage
        return garage;
    }
}