using Staticsoft.WsCommunication.Server.Abstractions;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Staticsoft.WsCommunication.Server.Local;

public class LocalWsServerConnection(
    WebSocket webSocket
)
{
    readonly WebSocket WebSocket = webSocket;

    public Task Send<T>(T message)
        => Send(new WsServerOutMessage<T>()
        {
            Type = typeof(T).Name,
            Body = message
        });

    Task Send<T>(WsServerOutMessage<T> message)
        => WebSocket.SendAsync(
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None
        );
}