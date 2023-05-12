using EnvironmentService.Models;

namespace EnvironmentService.Application.LogicContracts;

public interface IIndoorEnvironmentLogic
{
    public Task<IEnumerable<IndoorEnvironment>> GetAllIndoorEnvironmentsAsync();
    void TranslateData(string dataValue);
}