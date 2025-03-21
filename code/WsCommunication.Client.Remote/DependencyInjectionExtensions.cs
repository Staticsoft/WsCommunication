using Microsoft.Extensions.DependencyInjection;
using WsCommunication.Client.Abstractions;

namespace WsCommunication.Client.Remote;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseRemoteWsClient(
        this IServiceCollection services,
        Func<IServiceProvider, RemoteWsClient.Options> options
    )
        => services
            .AddSingleton<WsClient, RemoteWsClient>()
            .AddSingleton(options);
}