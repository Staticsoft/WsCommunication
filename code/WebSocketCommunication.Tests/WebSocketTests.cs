using System.Net.WebSockets;
using System.Text;

namespace Staticsoft.WebSocketCommunication.Tests;

public class WebSocketTests
{
    [Fact]
    public async Task CanConnectAndDisconnect()
    {
        var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri(Api), CancellationToken.None);
        var message = """
            { "Path": "/WebSocket/TestMessage", "Body": { "TestProperty": "Test value" } }
            """;
        await client.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "status description", CancellationToken.None);
    }

    string Api
        => Environment.GetEnvironmentVariable("WebSocketCommunicationApi")!;
}
