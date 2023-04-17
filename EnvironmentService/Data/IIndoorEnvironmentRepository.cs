using EnvironmentService.Models;

namespace EnvironmentService.Data;

public interface IndoorEnvironmentRepository
{
     Task<IEnumerable<IndoorEnvironment>> GetAllCarsAsync();
}