
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

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync()
    {
        var garages = await _repository.GetAllGaragesAsync();
        foreach (Garage garage in garages)
        {
            Console.WriteLine(garage.ToString());
        }
        return garages;
    }
}