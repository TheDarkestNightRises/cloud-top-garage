using CarService.Models;

namespace Contracts;

public record CarMoved(int carId, int garageId, int currentGarageId);
