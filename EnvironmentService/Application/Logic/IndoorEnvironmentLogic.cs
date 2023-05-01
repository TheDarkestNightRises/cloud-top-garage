using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Data;
using EnvironmentService.Models;

namespace EnvironmentService.Application.Logic;

public class IndoorEnvironmentLogic : IIndoorEnvironmentLogic
{
    private readonly IIndoorEnvironmentRepository _indoorEnvironmentRepository;
    private WebSocketSharp.WebSocketClient _webSocketClient;


    public IndoorEnvironmentLogic(IIndoorEnvironmentRepository indoorEnvironmentRepository, WebSocketSharp.WebSocketClient client)
     {
         _indoorEnvironmentRepository = indoorEnvironmentRepository;
         _webSocketClient = client;
         //_webSocketClient.Connect();
     }

    public async Task<IEnumerable<IndoorEnvironment>> GetAllIndoorEnvironmentsAsync()
    {
         _webSocketClient.Connect();
       return await _indoorEnvironmentRepository.GetAllIndoorEnvironmentsAsync();
    }
}
