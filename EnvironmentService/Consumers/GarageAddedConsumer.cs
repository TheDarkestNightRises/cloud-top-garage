using Contracts;
using EnvironmentService.Application.LogicContracts;
using MassTransit;

namespace EnvironmentService.Consumers;

public class GarageAddedConsumer : IConsumer<GarageCreated>
{
    private readonly IGarageLogic _garageLogic;


    public GarageAddedConsumer(IGarageLogic garageLogic)
    {
        _garageLogic = garageLogic;
    }

    public async Task Consume(ConsumeContext<GarageCreated> context)
    {
        try
        {
            var garageId = context.Message.garageId;
            Console.WriteLine($"--> GarageAddedConsumer: {garageId}");
            await _garageLogic.CreateGarageAsync(garageId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> GarageAddedConsumer: {e.Message}");
        }
    }
}
