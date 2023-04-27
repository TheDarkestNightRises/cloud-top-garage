using Contracts;
using GarageService.Application.LogicContracts;
using GarageService.Models;
using MassTransit;

namespace GarageService.Consumers;

public class CarUpdatedConsumer : IConsumer<CarUpdated>
{
    private readonly ICarLogic _carLogic;

    public CarUpdatedConsumer(ICarLogic carLogic)
    {
        _carLogic = carLogic;
    }
    public async Task Consume(ConsumeContext<CarUpdated> context)
    {
        try
        {
            var carCreatedMessage = context.Message.carToUpdate;
            Car car = new Car
            {
                Id = carCreatedMessage.Id
            };
            await _carLogic.UpdateCarAsync(car);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}