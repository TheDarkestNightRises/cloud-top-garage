using EnvironmentService.Models;

namespace EnvironmentService.Data;

public interface IIndoorEnvironmentRepository
{
    Task<IEnumerable<IndoorEnvironment>> GetAllIndoorEnvironmentsAsync();
    Task<IndoorEnvironment?> GetIndoorEnvironmentByMacAdress(int macAddress);
    Task<IndoorEnvironment> UpdateIndoorEnvironment(IndoorEnvironment indoorEnvironment);
    Task<IndoorEnvironment?> GetIndoorEnvironmentByIdAsync(int id);
}
