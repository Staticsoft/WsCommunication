using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.WsCommunication.Server.Local;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseLocalWsCommunication(this IServiceCollection services)
        => services
            .AddSingleton<WsServer, LocalWsServer>()
            .AddSingleton<LocalWsServerConnections>()
            .AddSingleton<LocalWsRequestHandler>();

    public static IApplicationBuilder UseLocalWsCommunication(this IApplicationBuilder app)
    {
        app.UseWebSockets();
        app.Use(async (HttpContext context, RequestDelegate next) =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var handler = context.RequestServices.GetRequiredService<LocalWsRequestHandler>();

                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await handler.Handle(webSocket);
            }
        });
        return app;
    }
}
