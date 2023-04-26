
using GarageService.Application.LogicContracts;
using GarageService.Models;
using GarageService.Data;

public class GarageLogic : IGarageLogic
{
    private readonly IGarageRepository _repository;

    public GarageLogic(IGarageRepository garageRepository)
    {
        _repository = garageRepository;
    }

    public async Task DeleteGarageAsync(int id)
    {
        // Get the garage to be deleted
        var garageToDelete = await _repository.GetGarageAsync(id);
        // check if the garage exists
        if (garageToDelete == null)
        {
            throw new Exception($"Garage with id {id} not found");
        }
        // delete the garage
        await _repository.DeleteGarageAsync(id);

    }

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync()
    {
        var garages = await _repository.GetAllGaragesAsync();
        foreach (Garage garage in garages)
        {
            Console.WriteLine(garage.ToString());
        }
        return garages;
    }
    public async Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageQuery garageQuery)
    {

        var garages = await _repository.GetAllGaragesAsync(garageQuery);
        foreach (Garage garage in garages)
        {
            Console.WriteLine(garage.ToString());
        }
        return garages;
    }
    public async Task<Garage?> GetGarageAsync(int id)
    {
        return await _repository.GetGarageAsync(id);
    }

}