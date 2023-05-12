using System;
using System.Text;
using EnvironmentService.Application.LogicContracts;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
namespace WebSocketSharp;
public class WebSocketClient
{
    private WebSocket _webSocket;

    public WebSocketClient(string url)
    {
        _webSocket = new WebSocket(url);
        _webSocket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        _webSocket.OnOpen += OnOpen;
        _webSocket.OnMessage += OnMessage;
        _webSocket.OnClose += OnClose;
    }

    public void Connect()
    {
        Console.WriteLine("--> WebSocket connecting");
        try
        {
            _webSocket.Connect();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine("--> WebSocket connected");
    }

    public void Send(byte[] data)
    {
        _webSocket.Send(data);
    }

    private void OnOpen(object sender, EventArgs e)
    {
        Console.WriteLine("--> WebSocket opened");
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        byte[] byteArray = e.RawData; // Assuming e.RawData is your byte array
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
        TranslateData(dataValue);
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        Console.WriteLine("--> WebSocket closed");
    }


    public void TranslateData(string dataValue)
    {
        // Extract the individual components from the hex string
        string c02Hex = dataValue.Substring(0, 4);
        string temperatureHex = dataValue.Substring(4, 4);
        string humidityHex = dataValue.Substring(8, 4);
        string lightHex = dataValue.Substring(12, 4); //Luminisoty per square meter
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
