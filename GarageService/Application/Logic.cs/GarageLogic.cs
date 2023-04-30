using GarageService.Application.LogicContracts;
using GarageService.Models;
using GarageService.Data;
using MassTransit;
using Contracts;
using System.ComponentModel.DataAnnotations;

public class GarageLogic : IGarageLogic
{
    private readonly IGarageRepository _garageRepository;
    private readonly IUserRepository _userRepository;
    
    private readonly IPublishEndpoint _publishEndpoint;

    public GarageLogic(IGarageRepository garageRepository, IUserRepository userRepository, IPublishEndpoint publishEndpoint)
    {
        _garageRepository = garageRepository;
        _userRepository = userRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task DeleteGarageAsync(int id)
    {
        // Get the garage to be deleted
        var garageToDelete = await _garageRepository.GetGarageAsync(id);
        // check if the garage exists
        if (garageToDelete == null)
        {
            throw new Exception($"Garage with id {id} not found");
        }
        // delete the garage
        await _garageRepository.DeleteGarageAsync(id);
    }

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync()
    {
        var garages = await _garageRepository.GetAllGaragesAsync();
        foreach (Garage garage in garages)
        {
            Console.WriteLine(garage.ToString());
        }
        return garages;
    }
    public async Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageQuery garageQuery)
    {

        var garages = await _garageRepository.GetAllGaragesAsync(garageQuery);
        foreach (Garage garage in garages)
        {
            Console.WriteLine(garage.ToString());
        }
        return garages;
    }
    public async Task<Garage?> GetGarageAsync(int id)
    {
        return await _garageRepository.GetGarageAsync(id);
    }
    public async Task<Garage> CreateGarageAsync(Garage garage)
    {
        ValidateGarage(garage);
        User? user = await _userRepository.GetUserByIdAsync(garage.User.Id);
        if (user is null)
        {
            throw new Exception($"User with id {garage.User.Id} not found");
        }
        garage.User = user;
        Garage garageCreated = await _garageRepository.CreateGarageAsync(garage);
        await _publishEndpoint.Publish(new GarageCreated(garage.Id));

        return garageCreated;
    }
    private void ValidateGarage(Garage car)
    {
        var validationContext = new ValidationContext(car);
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

        bool isValid = Validator.TryValidateObject(car, validationContext, validationResults, true);

        if (!isValid)
        {
            var firstError = validationResults.First();
            throw new ArgumentException(firstError.ErrorMessage);
        }
    }
}