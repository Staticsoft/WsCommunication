using Microsoft.Extensions.DependencyInjection;
using Staticsoft.WsCommunication.Client.Testing;

namespace Staticsoft.WsCommunication.Tests;

public class TestHostWebSocketTests : WebSocketTests
{
    protected override IServiceCollection ClientServices(IServiceCollection services)
        => base.ClientServices(services)
            .UseTestHostWsClient();

    protected override IServiceCollection ServerServices(IServiceCollection services)
        => base.ServerServices(services)
            .AddSingleton(_ => Client<HttpClient>());
}
