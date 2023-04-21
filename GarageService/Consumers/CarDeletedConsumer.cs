using Contracts;
using MassTransit;

namespace GarageService.Consumers;

public class CarDeletedConsumer : IConsumer<CarDeleted>
{
    public Task Consume(ConsumeContext<CarDeleted> context)
    {
        throw new NotImplementedException();
    }
}