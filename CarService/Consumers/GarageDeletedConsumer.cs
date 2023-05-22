using CarService.Application.LogicContracts;
using Contracts;
using MassTransit;

namespace GarageService.Consumers;

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
            var garageId = context.Message.GarageId;
            await _garageLogic.DeleteGarageAsync(garageId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
