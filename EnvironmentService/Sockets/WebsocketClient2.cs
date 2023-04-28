using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.WebSockets;
using Newtonsoft.Json.Linq;

namespace WebSocketClient;
class WebsocketClient : WebSocketHandler
{
    private WebSocket server = null;

    public WebsocketClient(int? maxIncomingMessageSize = 1024) : base(maxIncomingMessageSize)
    {
    }

    // Send down-link message to device
    // Must be in Json format according to https://github.com/ihavn/IoT_Semester_project/blob/master/LORA_NETWORK_SERVER.md
    public void SendDownLink(string jsonTelegram)
    {
        var echoBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonTelegram));
        server.SendAsync(echoBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    // E.g. url: "wss://iotnet.teracom.dk/app?token=??????????????????????????????????????????????="
    // Substitute ????????????????????? with the token you have been given
    public async Task ConnectAsync(string url)
    {
        using (var client = new ClientWebSocket())
        {
            await client.ConnectAsync(new Uri(url), CancellationToken.None);
            server = client;
            await OnOpen();
            while (client.State == WebSocketState.Open)
            {
                var buffer = new ArraySegment<byte>(new byte[1024]);
                var result = await client.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var json = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, result.Count);
                    await OnText(json);
                }
                else if (result.MessageType == WebSocketMessageType.Binary)
                {
                    var message = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, result.Count);
                    await OnBinary(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await OnClose(client.CloseStatus, client.CloseStatusDescription);
                }
            }
        }
    }

    private Task OnClose(WebSocketCloseStatus? closeStatus, string? closeStatusDescription)
    {
        Console.WriteLine($"WebSocket closed with status: {closeStatus}. Description: {closeStatusDescription}");
        return Task.CompletedTask;
    }

    public async Task OnOpen()
    {
        await server.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("WebSocket Listener has been opened for requests.")), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task OnText(string data)
    {
        var indented = JToken.Parse(data).ToString();
        Console.WriteLine(indented);
        await Task.CompletedTask;
    }

    public async Task OnBinary(string data)
    {
        Console.WriteLine($"Binary message received: {data}");
        await Task.CompletedTask;
    }


}

