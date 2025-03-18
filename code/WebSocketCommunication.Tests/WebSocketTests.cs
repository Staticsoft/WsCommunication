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
        await client.SendAsync(Encoding.UTF8.GetBytes(@"{""Test"": ""Message""}"), WebSocketMessageType.Text, true, CancellationToken.None);
        await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "status description", CancellationToken.None);
    }

    string Api
        => Environment.GetEnvironmentVariable("WebSocketCommunicationApi")!;
}
