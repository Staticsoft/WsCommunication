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

        endpoints.MapPost("/WebSocket/TestMessage", (WebSocketMessageRequest<TestMessage> request) =>
        {
            Console.WriteLine($"Received message: {request.ConnectionId} {request.Body.TestProperty}");
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

public class WebSocketMessageRequest<T>
{
    public required string ConnectionId { get; init; }
    public required T Body { get; init; }
}

public class TestMessage
{
    public required string TestProperty { get; init; }
}