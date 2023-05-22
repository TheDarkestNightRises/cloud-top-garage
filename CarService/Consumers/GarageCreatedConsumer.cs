using CarService.Application.LogicContracts;
using Contracts;
using MassTransit;

namespace GarageService.Consumers;

public class GarageCreatedConsumer : IConsumer<GarageCreated>
{
    private readonly IGarageLogic _garageLogic;

    public GarageCreatedConsumer(IGarageLogic garageLogic)
    {
        _garageLogic = garageLogic;
    }
    public async Task Consume(ConsumeContext<GarageCreated> context)
    {
        try
        {
            var garageId = context.Message.garageId;
            await _garageLogic.CreateGarageAsync(garageId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
