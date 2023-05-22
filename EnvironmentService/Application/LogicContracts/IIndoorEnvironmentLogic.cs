using EnvironmentService.Dtos;
using EnvironmentService.Models;

namespace EnvironmentService.Application.LogicContracts;

public interface IIndoorEnvironmentLogic
{
    public Task<IEnumerable<IndoorEnvironment>> GetAllIndoorEnvironmentsAsync();
    public void InitializeWebSockets();
    Task<IndoorEnvironment> UpdateSettingsAsync(int id, IndoorEnvironmentSettings newSettings);
}
