using Contracts;
using GarageService.Application.LogicContracts;
using GarageService.Models;
using MassTransit;

namespace GarageService.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly IUserLogic _userLogic;

    public UserCreatedConsumer(IUserLogic userLogic)
    {
        _userLogic = userLogic;
    }
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        try
        {
            var UserId = context.Message.userId;
            await _userLogic.CreateUserAsync(UserId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }


}
