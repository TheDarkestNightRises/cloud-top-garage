using Contracts;
using GarageService.Application.LogicContracts;
using MassTransit;

namespace GarageService.Consumers;

public class CarDeletedConsumer : IConsumer<CarDeleted>
{
    private readonly IGarageLogic _garageLogic;

    public CarDeletedConsumer(IGarageLogic garageLogic)
    {
        _garageLogic = garageLogic;
    }
    public Task Consume(ConsumeContext<CarDeleted> context)
    {
        throw new NotImplementedException();
    }
}