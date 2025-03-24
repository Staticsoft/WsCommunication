using Microsoft.Extensions.DependencyInjection;
using Staticsoft.WsCommunication.Client.Abstractions;
using Staticsoft.WsCommunication.Client.Remote;

namespace Staticsoft.WsCommunication.Tests;

public class ApiGatewayWebSocketTests : WebSocketTests
{
    protected override IServiceCollection ClientServices(IServiceCollection services)
        => base.ClientServices(services)
            .AddSingleton<WsClient, RemoteWsClient>()
            .AddSingleton(new RemoteWsClient.Options() { Uri = Configuration("WsCommunicationApi") });

    static string Configuration(string name)
        => Environment.GetEnvironmentVariable(name)
        ?? throw new NullReferenceException($"Environment varialbe {name} is not set");
}
