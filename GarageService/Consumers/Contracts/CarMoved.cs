using GarageService.Models;

namespace Contracts;

public record CarMoved(int carId, int garageId, int currentGarageId);
