using Contracts;
using MassTransit;

namespace GarageService.Consumers;

public class HelloEventConsumer : IConsumer<HelloEvent>
{
    public async Task Consume(ConsumeContext<HelloEvent> context)
    {
        Console.WriteLine(context.Message);
        await Task.FromResult(context);
    }
}
