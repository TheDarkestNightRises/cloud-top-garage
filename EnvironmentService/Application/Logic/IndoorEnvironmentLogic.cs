using System.Text;
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

    public void TranslateData(string dataValue)
    {
        // Extract the individual components from the hex string
        string c02Hex = dataValue.Substring(0, 4);
        string temperatureHex = dataValue.Substring(4, 4);
        string humidityHex = dataValue.Substring(8, 4);
        string lightHex = dataValue.Substring(12, 4);
        string motionAlarmHex = dataValue.Substring(16, 2);
        string idMacAddressHex = dataValue.Substring(18, 2);

        // Convert hex strings to decimal values
        int c02 = Convert.ToInt32(c02Hex, 16);
        int temperature = Convert.ToInt32(temperatureHex, 16);
        int humidity = Convert.ToInt32(humidityHex, 16);
        int light = Convert.ToInt32(lightHex, 16);
        int motionAlarm = Convert.ToInt32(motionAlarmHex, 16);
        int idMacAddress = Convert.ToInt32(idMacAddressHex, 16);

        // Print the translated values
        Console.WriteLine($"CO2: {c02}");
        Console.WriteLine($"Temperature: {temperature}");
        Console.WriteLine($"Humidity: {humidity}");
        Console.WriteLine($"Light: {light}");
        Console.WriteLine($"Motion/Alarm: {motionAlarm}");
        Console.WriteLine($"ID/MAC Address: {idMacAddress}");
    }
}
