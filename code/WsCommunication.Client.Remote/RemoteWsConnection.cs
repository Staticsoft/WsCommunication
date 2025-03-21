using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WsCommunication.Client.Abstractions;

namespace WsCommunication.Client.Remote;

public class RemoteWsConnection(
    ClientWebSocket client
) : WsConnection
{
    readonly ClientWebSocket Client = client;

    public async Task<string> Receive()
    {
        var bytes = new List<byte>();
        var end = false;
        while (!end)
        {
            var buffer = new byte[4096];
            var received = await Client.ReceiveAsync(buffer, CancellationToken.None);
            bytes.AddRange(buffer[..received.Count]);

            end = received.EndOfMessage;
        }
        return Encoding.UTF8.GetString(bytes.ToArray());
    }

    public Task Send<T>(WsClientMessage<T> message)
    {
        var body = $$"""
            {"Path":"{{message.Path}}","Body":{{JsonSerializer.Serialize(message.Body)}}}
            """;
        var bytes = Encoding.UTF8.GetBytes(body);
        return Client.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async ValueTask DisposeAsync()
    {
        await Client.CloseAsync(WebSocketCloseStatus.NormalClosure, nameof(WebSocketCloseStatus.NormalClosure), CancellationToken.None);
        Client.Dispose();
    }
}
