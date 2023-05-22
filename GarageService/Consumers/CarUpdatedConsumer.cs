using Contracts;
using GarageService.Application.LogicContracts;
using GarageService.Models;
using MassTransit;

namespace GarageService.Consumers;

public class CarUpdatedConsumer : IConsumer<CarMoved>
{
    private readonly ICarLogic _carLogic;

    public CarUpdatedConsumer(ICarLogic carLogic)
    {
        _carLogic = carLogic;
    }
    public async Task Consume(ConsumeContext<CarMoved> context)
    {
        try
        {
            var carId = context.Message.carId;
            var garageId = context.Message.garageId;
            var currentGarageId = context.Message.currentGarageId;
            await _carLogic.UpdateCarAsync(carId, garageId, currentGarageId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
