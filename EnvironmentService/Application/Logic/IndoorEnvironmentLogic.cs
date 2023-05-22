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
            WebSocket client = new WebSocket(environment.IndoorEnvironmentSettings.LoRaWANURL);
            client.OnMessage += async (sender, e) => await OnMessageFromIotAsync(e.RawData);
            client.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            Console.WriteLine($"Connecting environment {environment.Id} to {environment.IndoorEnvironmentSettings.LoRaWANURL}");
            client.Connect();
            _webSocketClients.Add(environment.Id, client);
        }
        Console.WriteLine("WebSocket Clients:");
        foreach (var kvp in _webSocketClients)
        {
            Console.WriteLine($"ID: {kvp.Key}, WebSocket: {kvp.Value}");
        }
    }



    private async Task OnMessageFromIotAsync(byte[] data)
    {
        Console.WriteLine($"Message received from IoT: {data}");
        try 
        {
            var convertedData = ConvertDataIntoString(data);
            var stat = await TranslateDataAsync(convertedData);
            await _statRepository.AddStatAsync(stat);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }
        
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
        newSettings.LoRaWANURL = oldSettings.LoRaWANURL;
        SendSettingsAsync(id,newSettings);
        return indoorEnvironmentUpdated;
    }

    private void SendSettingsAsync(int id,IndoorEnvironmentSettings newSettings)
    {
        byte[] settingsByte = ConvertSettingsToByteArray(newSettings);
        Console.WriteLine("WebSocket Clients in send:");
        WebSocket client = new WebSocket(newSettings.LoRaWANURL);
        client.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        client.Connect();
        if (client.ReadyState == WebSocketState.Open) 
        {
            Console.WriteLine($"-->Sending settings to the iot device {settingsByte}");
            client.Send(settingsByte);
            client.Close();
        }
        else
        {
            throw new ArgumentException("Iot device not open");
        }
    }
    private byte[] ConvertSettingsToByteArray(IndoorEnvironmentSettings settings)
    {
        byte[] settingsByte = new byte[10]; 
        settingsByte[0] = (byte)(settings.Co2Limit >> 8); 
        settingsByte[1] = (byte)(settings.Co2Limit & 0xFF);
        settingsByte[2] = (byte)(settings.TemperatureLimit >> 8); 
        settingsByte[3] = (byte)(settings.TemperatureLimit & 0xFF);
        settingsByte[4] = (byte)(settings.HumidityLimit >> 8);
        settingsByte[5] = (byte)(settings.HumidityLimit & 0xFF);
        settingsByte[6] = (byte)(settings.LightLimit >> 8);
        settingsByte[7] = (byte)(settings.LightLimit & 0xFF);
        return settingsByte;
    }
}


