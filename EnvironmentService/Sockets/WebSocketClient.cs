using System;
using WebSocketSharp;
namespace WebSocketSharp;
public class WebSocketClient
{
    private WebSocket _webSocket;

    public WebSocketClient(string url)
    {
        _webSocket = new WebSocket(url);
        _webSocket.OnOpen += OnOpen;
        _webSocket.OnMessage += OnMessage;
        _webSocket.OnClose += OnClose;
    }

    public void Connect()
    {
         Console.WriteLine("WebSocket connecting");
        _webSocket.Connect();
        Console.WriteLine("WebSocket connected");
    }

    public void Send(byte[] data)
    {
        _webSocket.Send(data);
    }

    private void OnOpen(object sender, EventArgs e)
    {
        Console.WriteLine("WebSocket opened");
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        Console.WriteLine($"Received data: {BitConverter.ToString(e.RawData)}");
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        Console.WriteLine("WebSocket closed");
    }
}
