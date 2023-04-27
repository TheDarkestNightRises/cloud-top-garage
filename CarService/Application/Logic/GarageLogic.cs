
using CarService.Application.LogicContracts;
using CarService.Data;
using CarService.Models;

public class GarageLogic : IGarageLogic
{
    private readonly IGarageRepository _garageRepository;


    public GarageLogic(IGarageRepository garageRepository)
    {
        _garageRepository = garageRepository;

    }

    public async Task DeleteGarageAsync(int id)
    {
        // Check if the garage exists in the repository
        var garage = await _garageRepository.GetGarageAsync(id);
        if (garage == null)
        {
            throw new Exception($"Garage with id {id} not found");
        }

        // Delete the garage from the repository
        await _garageRepository.DeleteGarageAsync(id);

    }

    public Task<Garage?> GetGarageAsync(int id)
    {
        throw new NotImplementedException();
    }
}