using GarageService.Models;

namespace Contracts;

public record CarUpdated(Car carToUpdate); //Car Moved to another garage
//public record CarMoved(int garageId, int carId)