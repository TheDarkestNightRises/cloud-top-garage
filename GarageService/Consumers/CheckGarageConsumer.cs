using Contracts;
using GarageService.Application.LogicContracts;
using GarageService.Models;
using MassTransit;

namespace GarageService.Consumers;

public class CheckGarageConsumer : IConsumer<CheckGarage>
{

    private readonly IGarageLogic _garageLogic;

    public CheckGarageConsumer(IGarageLogic garageLogic)
    {
        _garageLogic = garageLogic;
    }

    public async Task Consume(ConsumeContext<CheckGarage> context)
    {
        try
        {
            var garageId = context.Message.Id;
            Garage? garage = await _garageLogic.GetGarageAsync(garageId);  
            await context.RespondAsync(new GarageChecked(garage != null));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
