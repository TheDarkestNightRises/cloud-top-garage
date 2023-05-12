using System.Text;
using EnvironmentService.Application.LogicContracts;
using EnvironmentService.Data;
using EnvironmentService.Models;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace EnvironmentService.Application.Logic;

public class IndoorEnvironmentLogic : IIndoorEnvironmentLogic
{
    private readonly IIndoorEnvironmentRepository _indoorEnvironmentRepository;
    private readonly IStatRepository _statRepository;
    private Dictionary<int, WebSocket> _webSocketClients;

    public IndoorEnvironmentLogic(IIndoorEnvironmentRepository indoorEnvironmentRepository, IStatRepository statRepository)
    {
        _indoorEnvironmentRepository = indoorEnvironmentRepository;
        _statRepository = statRepository;
        _webSocketClients = new Dictionary<int, WebSocket>();
    }

    public async void InitializeWebSockets()
    {
        Console.WriteLine("Initializing WebSockets...");
        IEnumerable<IndoorEnvironment> indoorEnvironments = await _indoorEnvironmentRepository.GetAllIndoorEnvironmentsAsync();

        foreach (var environment in indoorEnvironments)
        {
            // Create a WebSocket client for each IndoorEnvironment
            WebSocket client = new WebSocket(environment.LoRaWANURL);
            client.OnMessage += async (sender, e) => await OnMessageFromIotAsync(e.RawData);
            client.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            Console.WriteLine($"Connecting environment {environment.Id} to {environment.LoRaWANURL}");
            client.Connect();

            _webSocketClients.Add(environment.Id, client);
        }
    }



    private async Task OnMessageFromIotAsync(byte[] data)
    {
        Console.WriteLine($"Message received from IoT: {data}");
        var convertedData = ConvertDataIntoString(data);
        var stat = await TranslateDataAsync(convertedData);
        await _statRepository.AddStatAsync(stat);
    }

    public async Task<IEnumerable<IndoorEnvironment>> GetAllIndoorEnvironmentsAsync()
    {
        return await _indoorEnvironmentRepository.GetAllIndoorEnvironmentsAsync();
    }

    private async Task<Stat> TranslateDataAsync(string dataValue)
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
        float temperature = Convert.ToInt32(temperatureHex, 16) / 10f;
        float humidity = Convert.ToInt32(humidityHex, 16) / 10f;
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

        Stat stat = new Stat()
        {
            CO2 = c02,
            Temperature = temperature,
            Humidity = humidity,
            IndoorEnvironment = await _indoorEnvironmentRepository.GetIndoorEnvironmentByMacAdress(idMacAddress)
        };

        return stat;
    }

    private String ConvertDataIntoString(byte[] byteArray)
    {
        string byteArrayString = BitConverter.ToString(byteArray);
        string[] hexValues = byteArrayString.Split('-');
        byte[] newByteArray = new byte[hexValues.Length];

        for (int i = 0; i < hexValues.Length; i++)
        {
            newByteArray[i] = byte.Parse(hexValues[i], System.Globalization.NumberStyles.HexNumber);
        }

        string result = Encoding.UTF8.GetString(newByteArray);
        var jsonObject = JObject.Parse(result);
        var dataValue = (string)jsonObject["data"];
        Console.WriteLine($"data: {dataValue}");
        return dataValue;
    }


}


