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

    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        Console.WriteLine("--> WebSocket closed");
    }
}
