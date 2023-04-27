using Contracts;
using GarageService.Application.LogicContracts;
using GarageService.Models;
using MassTransit;

namespace GarageService.Consumers;

public class CarCreatedConsumer : IConsumer<CarCreated>
{
    private readonly ICarLogic _carLogic;

    public CarCreatedConsumer(ICarLogic carLogic)
    {
        _carLogic = carLogic;
    }
    public async Task Consume(ConsumeContext<CarCreated> context)
    {
        try
        {
            var CarId = context.Message.carId;
            var GarageId = context.Message.garageId;
            await _carLogic.CreateCarAsync(CarId,GarageId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


}