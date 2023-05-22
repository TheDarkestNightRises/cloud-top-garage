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
    private List<WebSocket> _webSocketClients = new List<WebSocket>();

    public IndoorEnvironmentLogic(IIndoorEnvironmentRepository indoorEnvironmentRepository, IStatRepository statRepository)
    {
        _indoorEnvironmentRepository = indoorEnvironmentRepository;
        _statRepository = statRepository;
    }

    public async void InitializeWebSockets()
    {
        Console.WriteLine("Initializing WebSockets...");
        IEnumerable<IndoorEnvironment> indoorEnvironments = await _indoorEnvironmentRepository.GetAllIndoorEnvironmentsAsync();

        foreach (var environment in indoorEnvironments)
        {
            // Create a WebSocket client for each IndoorEnvironment
            WebSocket client = new WebSocket(environment.IndoorEnvironmentSettings.LoRaWANURL);
            client.OnMessage += async (sender, e) => await OnMessageFromIotAsync(e.RawData);
            client.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            Console.WriteLine($"Connecting environment {environment.Id} to {environment.IndoorEnvironmentSettings.LoRaWANURL}");
            client.Connect();
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

    public async Task<IndoorEnvironment> UpdateSettingsAsync(int id, IndoorEnvironmentSettings newSettings)
    {
        var indoorEnvironment = await _indoorEnvironmentRepository.GetIndoorEnvironmentByIdAsync(id);

        if (indoorEnvironment is null)
        {
            throw new ArgumentException($"Indoor environment with id: {id} not found");
        }

        var oldSettings = indoorEnvironment.IndoorEnvironmentSettings;
        (oldSettings.Co2Limit,oldSettings.HumidityLimit,oldSettings.LightLimit,oldSettings.LightOn,oldSettings.TemperatureLimit)
        = (newSettings.Co2Limit,newSettings.HumidityLimit,newSettings.LightLimit,newSettings.LightOn,newSettings.TemperatureLimit);
        //Send data to IOT
        var indoorEnvironmentUpdated = await _indoorEnvironmentRepository.UpdateIndoorEnvironment(indoorEnvironment);
        await SendSettingsAsync(id,newSettings);
        return indoorEnvironmentUpdated;
    }

    private async Task SendSettingsAsync(int id,IndoorEnvironmentSettings newSettings)
    {
        byte[] settingsByte = new byte[10]; 
        settingsByte[0] = (byte)(newSettings.Co2Limit >> 8); 
        settingsByte[1] = (byte)(newSettings.Co2Limit & 0xFF);
        settingsByte[2] = (byte)(newSettings.TemperatureLimit >> 8); 
        settingsByte[3] = (byte)(newSettings.TemperatureLimit & 0xFF);
        settingsByte[4] = (byte)(newSettings.HumidityLimit >> 8);
        settingsByte[5] = (byte)(newSettings.HumidityLimit & 0xFF);
        settingsByte[6] = (byte)(newSettings.LightLimit >> 8);
        settingsByte[7] = (byte)(newSettings.LightLimit & 0xFF);
        WebSocket client = _webSocketClients[id];
        if (client.ReadyState == WebSocketState.Open) 
        {
            Console.WriteLine($"-->Sending settings to the iot device {settingsByte}");
            client.Send(settingsByte);
        }
        else
        {
            throw new ArgumentException("Iot device not open");
        }
    }
}


