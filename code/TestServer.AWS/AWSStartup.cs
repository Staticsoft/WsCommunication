namespace Staticsoft.TestServer.AWS;

public class AWSStartup : Startup
{
    protected override IServiceCollection RegisterServices(IServiceCollection services)
        => base.RegisterServices(services);

    static string Configuration(string name)
        => Environment.GetEnvironmentVariable(name)
        ?? throw new NullReferenceException($"Environment varialbe {name} is not set");

    protected override IEndpointRouteBuilder ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/WebSocket/Connect", (WebSocketConnectRequest request) =>
        {
            Console.WriteLine($"Received connect request: {request.ConnectionId}");
        });
        endpoints.MapPost("/WebSocket/Disconnect", (WebSocketDisconnectRequest request) =>
        {
            Console.WriteLine($"Received disconnect request: {request.ConnectionId}");
        });
        endpoints.MapPost("/WebSocket/Message", async (WebSocketMessageRequest request) =>
        {

        });
        return base.ConfigureEndpoints(endpoints);
    }
}

public class WebSocketConnectRequest
{
    public required string ConnectionId { get; init; }
}

public class WebSocketDisconnectRequest
{
    public required string ConnectionId { get; init; }
}

public class WebSocketMessageRequest
{
    public required string ConnectionId { get; init; }
    public required string Body { get; init; }
}