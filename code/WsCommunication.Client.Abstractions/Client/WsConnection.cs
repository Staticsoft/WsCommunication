using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Staticsoft.WsCommunication.Client.Abstractions;

public class WsConnection(
    WebSocket webSocket
)
{
    readonly WebSocket WebSocket = webSocket;

    public Task Send<T>(WsClientOutMessage<T> message)
    {
        var body = $$"""
            {"Path":"{{message.Path}}","Body":{{JsonSerializer.Serialize(message.Body)}}}
            """;
        var bytes = Encoding.UTF8.GetBytes(body);
        return WebSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task<string> Receive()
    {
        var bytes = new List<byte>();
        var end = false;
        while (!end)
        {
            var buffer = new byte[4096];
            var received = await WebSocket.ReceiveAsync(buffer, CancellationToken.None);
            bytes.AddRange(buffer[..received.Count]);

            end = received.EndOfMessage;
        }
        return Encoding.UTF8.GetString(bytes.ToArray());
    }

    public async ValueTask DisposeAsync()
    {
        await WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, nameof(WebSocketCloseStatus.NormalClosure), CancellationToken.None);
        WebSocket.Dispose();
    }
}
