using Contracts;
using EnvironmentService.Application.LogicContracts;
using MassTransit;

namespace EnvironmentService.Consumers;

public class GarageDeletedConsumer : IConsumer<GarageDeleted>
{
    private readonly IGarageLogic _garageLogic;


    public GarageDeletedConsumer(IGarageLogic garageLogic)
    {
        _garageLogic = garageLogic;
    }

    public async Task Consume(ConsumeContext<GarageDeleted> context)
    {
        try
        {
            var garageId = context.Message.garageId;
            Console.WriteLine($"--> GarageDeletedConsumer: {garageId}");
            await _garageLogic.DeleteGarageAsync(garageId);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> GarageDeletedConsumer: {e.Message}");
        }
    }
}