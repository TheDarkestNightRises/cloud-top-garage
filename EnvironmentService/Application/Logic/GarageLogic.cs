using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Data;
using EnvironmentService.Models;

namespace EnvironmentService.Application.Logic;

public class GarageLogic : IGarageLogic
{
    private readonly IGarageRepository _garageRepository;

    public GarageLogic(IGarageRepository garageRepository)
    {
        _garageRepository = garageRepository;
    }

    public async Task<Garage> CreateGarageAsync(int garageId) 
    {
        var garageFound = await _garageRepository.GetGarageByIdAsync(garageId);
        if (garageFound is not null)
        {
            throw new Exception($"Garage with id {garageId} already exists");
        }
        var garage = new Garage
        {
            Id = garageId
        };
        return await _garageRepository.CreateGarageAsync(garage);
    }
    
    public async Task DeleteGarageAsync(int garageId)
    {
        var garageFound = await _garageRepository.GetGarageByIdAsync(garageId);
        if (garageFound is null)
        {
            throw new Exception($"Garage with id {garageId} does not exist");
        }
        await _garageRepository.DeleteGarageAsync(garageFound);
    }
}