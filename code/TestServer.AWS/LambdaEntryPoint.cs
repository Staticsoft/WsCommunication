using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Staticsoft.Asp.Lambda;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Staticsoft.TestServer.AWS;

public class LambdaEntryPoint : LambdaEntrypoint<AWSStartup>
{
    protected override TriggerCollection Triggers => base.Triggers
        .AddTrigger<CloudWatchTrigger>()
        .AddTrigger<RequestTrigger>()
        .AddTrigger<WebSocketTrigger>();
}

public class WebSocketTrigger : TriggerSource
{
    public bool TryConvert(
        JsonElement request,
        JsonSerializerOptions options,
        [NotNullWhen(true)] out APIGatewayProxyRequest? proxyRequest
    )
    {
        proxyRequest = null;
        Console.WriteLine($"Request: {request.GetRawText()}");

        if (request.TryGetProperty("requestContext", out var context))
        {
            if (context.TryGetProperty("eventType", out var eventType)
             && context.TryGetProperty("connectionId", out var connectionId))
            {
                proxyRequest = (eventType.GetString(), connectionId.GetString()) switch
                {
                    ("CONNECT", string id) => ConnectRequest(id),
                    ("DISCONNECT", string id) => DisconnectRequest(id),
                    ("MESSAGE", string id) => ConnectRequest(id),
                    _ => null
                };
            }
        }
        Console.WriteLine($"ProxyRequest: {JsonSerializer.Serialize(proxyRequest)}");
        return proxyRequest != null;
    }

    static APIGatewayProxyRequest ConnectRequest(string connectionId)
        => new()
        {
            HttpMethod = "POST",
            Path = "/WebSocket/Connect",
            Resource = "/WebSocket/Connect",
            Body = $@"{{""ConnectionId"":""{connectionId}""}}",
            Headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "Accepts", "application/json" }
            }
        };

    static APIGatewayProxyRequest DisconnectRequest(string connectionId)
        => new()
        {
            HttpMethod = "POST",
            Path = "/WebSocket/Disconnect",
            Resource = "/WebSocket/Disconnect",
            Body = $@"{{""ConnectionId"":""{connectionId}""}}",
            Headers = new Dictionary<string, string>()
            {
                { "Content-Type", "application/json" },
                { "Accepts", "application/json" }
            }
        };
}