using Staticsoft.TestContracts;
using Staticsoft.Testing.Integration;
using Staticsoft.TestServer.Local;
using WsCommunication.Client.Abstractions;

namespace Staticsoft.WebSocketCommunication.Tests;

public abstract class WebSocketTests : IntegrationTestBase<LocalStartup>
{
    WsClient Client => Client<WsClient>();

    [Fact(Timeout = 5_000)]
    public async Task CanConnectSendReceiveDisconnect()
    {
        await using var connection = await Client.Connect();

        await connection.Send<TestMessage>(new()
        {
            Path = "/WebSocket/TestMessage",
            Body = new() { TestProperty = "Test value" }
        });

        var text = await connection.Receive();

        Assert.Equal("Test value", text);
    }
}
