
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

    public Task DeleteGarageAsync(int id)
    {
        throw new NotImplementedException();
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