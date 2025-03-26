using Amazon.ApiGatewayManagementApi;
using Microsoft.Extensions.DependencyInjection;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.WsCommunication.Server.ApiGateway;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseApiGatewayWsServer(
        this IServiceCollection services,
        Func<IServiceProvider, AmazonApiGatewayManagementApiClient> client,
        Func<IServiceProvider, AmazonApiGatewayManagementApiConfig> config
    )
        => services
            .AddSingleton<WsServer, ApiGatewayWsServer>()
            .AddSingleton(client)
            .AddSingleton(config);
}