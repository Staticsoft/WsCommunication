using Microsoft.Extensions.DependencyInjection;
using Staticsoft.WsCommunication.Client.Abstractions;

namespace Staticsoft.WsCommunication.Tests;

public class TestHostWebSocketTests : WebSocketTests
{
    protected override IServiceCollection ClientServices(IServiceCollection services)
        => base.ClientServices(services)
            .AddSingleton<WsClient, TestHostWsClient>();
}
