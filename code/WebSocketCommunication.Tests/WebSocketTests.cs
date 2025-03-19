using System.Net.WebSockets;
using System.Text;

namespace Staticsoft.WebSocketCommunication.Tests;

public class WebSocketTests
{
    [Fact(Timeout = 5_000)]
    public async Task CanConnectAndDisconnect()
    {
        var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri(Api), CancellationToken.None);
        var message = """
            { "Path": "/WebSocket/TestMessage", "Body": { "TestProperty": "Test value" } }
            """;
        await client.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);

        var bytes = new List<byte>();
        var end = false;
        while (!end)
        {
            var buffer = new byte[4096];
            var received = await client.ReceiveAsync(buffer, CancellationToken.None);
            bytes.AddRange(buffer[..received.Count]);

            end = received.EndOfMessage;
        }
        var text = Encoding.UTF8.GetString(bytes.ToArray());
        Assert.Equal("Test value", text);

        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "status description", CancellationToken.None);
    }

    string Api
        => Environment.GetEnvironmentVariable("WebSocketCommunicationApi")!;
}
