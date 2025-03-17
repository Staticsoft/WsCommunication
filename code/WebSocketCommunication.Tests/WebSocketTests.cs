using System.Net.WebSockets;

namespace Staticsoft.WebSocketCommunication.Tests;

public class WebSocketTests
{
    [Fact]
    public async Task CanConnectAndDisconnect()
    {
        var client = new ClientWebSocket();
        await client.ConnectAsync(new Uri(Api), CancellationToken.None);
        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "status description", CancellationToken.None);
    }

    string Api
        => Environment.GetEnvironmentVariable("WebSocketCommunicationApi")!;
}
