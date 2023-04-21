using Contracts;
using GarageService.Application.LogicContracts;
using MassTransit;

namespace GarageService.Consumers;

public class CarDeletedConsumer : IConsumer<CarDeleted>
{
    private readonly ICarLogic _carLogic;

    public CarDeletedConsumer(ICarLogic carLogic)
    {
        _carLogic = carLogic;
    }
    public async Task Consume(ConsumeContext<CarDeleted> context)
    {
        var carId = context.Message.Id;
        var car = _carLogic.GetCarByIdAsync(carId);

        if (car == null)
        {
            Console.WriteLine($"Car with id {carId} not found");
            return;
        }
        await _carLogic.DeleteCarAsync(carId);
    }
}