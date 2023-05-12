using EnvironmentService.Models;

namespace EnvironmentService.Data;

public interface IIndoorEnvironmentRepository
{
    Task<IEnumerable<IndoorEnvironment>> GetAllIndoorEnvironmentsAsync();
    Task<IndoorEnvironment?> GetIndoorEnvironmentByMacAdress(int macAddress);
}