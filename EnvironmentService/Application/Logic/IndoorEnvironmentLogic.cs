using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Data;
using EnvironmentService.Models;

namespace EnvironmentService.Application.Logic;

public class IndoorEnvironmentLogic : IIndoorEnvironmentLogic
{
    private readonly IIndoorEnvironmentRepository _indoorEnvironmentRepository;

    public IndoorEnvironmentLogic(IIndoorEnvironmentRepository indoorEnvironmentRepository)
     {
        _indoorEnvironmentRepository = indoorEnvironmentRepository;
     }

    public async Task<IEnumerable<IndoorEnvironment>> GetAllIndoorEnvironmentsAsync()
    {
       return await _indoorEnvironmentRepository.GetAllIndoorEnvironmentsAsync();
    }
}
