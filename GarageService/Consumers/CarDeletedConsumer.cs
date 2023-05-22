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
        try
        {
            var carId = context.Message.Id;
            await _carLogic.DeleteCarAsync(carId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
