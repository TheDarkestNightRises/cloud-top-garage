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
            var carCreatedMessage = context.Message.car;
            Car car = new Car
            {
                Id = carCreatedMessage.Id,
                Name = carCreatedMessage.Name,
                Description = carCreatedMessage.Description,
                Image = carCreatedMessage.Image,
                Garage = carCreatedMessage.Garage
                
            };
            await _carLogic.CreateCarAsync(car);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


}