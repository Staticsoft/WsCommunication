using Staticsoft.TestContracts;
using Staticsoft.Testing.Integration;
using Staticsoft.TestServer.Local;
using Staticsoft.WsCommunication.Client.Abstractions;

namespace Staticsoft.WsCommunication.Tests;

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

        var testMessages = connection.Receive<TestMessage>();
        var message = await testMessages.ReadAsync();

        Assert.Equal("Test value", message.TestProperty);
    }
}
