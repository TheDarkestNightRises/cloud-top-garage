using System.ComponentModel.DataAnnotations;
using Contracts;
using GarageService.Application.LogicContracts;
using GarageService.Data;
using GarageService.Models;
using MassTransit;

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
        await _publishEndpoint.Publish(new GarageDeleted(garageToDelete.Id));
        await _garageRepository.DeleteGarageAsync(id);
    }

    public async Task<IEnumerable<Garage>> GetAllGaragesAsync()
    {
        // Retrieve all garages
        var garages = await _garageRepository.GetAllGaragesAsync();
        foreach (Garage garage in garages)
        {
            Console.WriteLine(garage.ToString());
        }
        // return garages
        return garages;
    }
    public async Task<IEnumerable<Garage>> GetAllGaragesAsync(GarageQuery garageQuery)
    {
        // Retrieve all garages
        var garages = await _garageRepository.GetAllGaragesAsync(garageQuery);
        foreach (Garage garage in garages)
        {
            Console.WriteLine(garage.ToString());
        }
        // return garages
        return garages;
    }
    public async Task<Garage?> GetGarageAsync(int id)
    {
        // Get a garage with a specific id
        return await _garageRepository.GetGarageAsync(id);
    }
    public async Task<Garage> CreateGarageAsync(Garage garage)
    {
        // Validates the inputs of the new garage in order to create it
        ValidateGarage(garage);
        // Get the user that will own the garage
        User? user = await _userRepository.GetUserByIdAsync(garage.User.Id);
        // Check if the user that will own the garage exists
        if (user is null)
        {
            throw new Exception($"User with id {garage.User.Id} not found");
        }
        // Appoint the user as the owner of the new garage
        garage.User = user;
        // Create the new garage
        Garage garageCreated = await _garageRepository.CreateGarageAsync(garage);
        // Sends a message to the car service to notify it that a new garage has been created
        await _publishEndpoint.Publish(new GarageCreated(garage.Id));
        // Returns the created garage
        return garageCreated;
    }
    private void ValidateGarage(Garage garage)
    {
        // Setup validation environment
        var validationContext = new ValidationContext(garage);
        // Setup results environment
        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        // Checks if the validation of the garage object matches the expected results
        bool isValid = Validator.TryValidateObject(garage, validationContext, validationResults, true);
        // If the object is not valid throw the first given exception
        if (!isValid)
        {
            var firstError = validationResults.First();
            throw new ArgumentException(firstError.ErrorMessage);
        }
    }
}
