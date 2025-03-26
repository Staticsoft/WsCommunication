using Microsoft.Extensions.DependencyInjection;
using Staticsoft.WsCommunication.Client.Abstractions;

namespace Staticsoft.WsCommunication.Client.Testing;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseTestHostWsClient(this IServiceCollection services)
        => services
            .AddSingleton<WsClient, TestHostWsClient>();
}
