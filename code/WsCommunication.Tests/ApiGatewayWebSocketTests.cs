using Microsoft.Extensions.DependencyInjection;
using WsCommunication.Client.Abstractions;
using WsCommunication.Client.Remote;

namespace Staticsoft.WebSocketCommunication.Tests;

public class ApiGatewayWebSocketTests : WebSocketTests
{
    protected override IServiceCollection ClientServices(IServiceCollection services)
        => base.ClientServices(services)
            .AddSingleton<WsClient, RemoteWsClient>()
            .AddSingleton(new RemoteWsClient.Options() { Uri = Configuration("WebSocketCommunicationApi") });

    static string Configuration(string name)
        => Environment.GetEnvironmentVariable(name)
        ?? throw new NullReferenceException($"Environment varialbe {name} is not set");
}
