namespace GarageService.Application.LogicContracts;

public interface IUserLogic
{
    Task CreateUserAsync(int userId);
}