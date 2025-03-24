using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.WsCommunication.Server.ASP;

public static class DependencyInjectionExtensions
{
    const string Connect = "/WebSocket/Connect";
    const string Disconnect = "/WebSocket/Disconnect";

    public static IApplicationBuilder UseDefaultWsHandlers(this IApplicationBuilder builder)
        => builder.UseEndpoints(UseDefaultWsHandlers);

    static void UseDefaultWsHandlers(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Connect, (WsConnectRequest request, ILogger<WsConnectRequest> logger) =>
            logger.LogRequest(Connect, request.ConnectionId)
        );
        builder.MapPost(Disconnect, (WsDisconnectRequest request, ILogger<WsDisconnectRequest> logger) =>
            logger.LogRequest(Disconnect, request.ConnectionId)
        );
    }

    static void LogRequest(this ILogger logger, string request, string connectionId)
        => logger.LogInformation("{request} request received. ConnectionId: {connectionId}", request, connectionId);
}
