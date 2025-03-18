using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Authentication;
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
    static readonly Dictionary<string, string> Headers = new()
    {
        { "Content-Type", "application/json" },
        { "Accepts", "application/json" }
    };

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
            proxyRequest = (context.GetString("eventType"), context.GetString("connectionId")) switch
            {
                ("CONNECT", string id) => ConnectRequest(id),
                ("DISCONNECT", string id) => DisconnectRequest(id),
                ("MESSAGE", string id) when request.GetString("body") is string body => MessageRequest(id, body),
                _ => null
            };
        }
        Console.WriteLine($"ProxyRequest: {JsonSerializer.Serialize(proxyRequest)}");
        return proxyRequest != null;
    }

    static APIGatewayProxyRequest ConnectRequest(string connectionId)
        => Request("/WebSocket/Connect", $@"{{""ConnectionId"":""{connectionId}""}}");

    static APIGatewayProxyRequest DisconnectRequest(string connectionId)
        => Request("/WebSocket/Disconnect", $@"{{""ConnectionId"":""{connectionId}""}}");

    static APIGatewayProxyRequest? MessageRequest(string connectionId, string body)
        => JsonSerializer.Deserialize<WebSocketMessageRequest>(body) switch
        {
            WebSocketMessageRequest request => MessageRequest(connectionId, request),
            _ => null
        };

    static APIGatewayProxyRequest MessageRequest(string connectionId, WebSocketMessageRequest request)
        => Request(request.Path, $@"{{""ConnectionId"":""{connectionId}"",""Body"":{request.Body.GetRawText()}}}");

    static APIGatewayProxyRequest Request(string path, string body)
        => new()
        {
            HttpMethod = "POST",
            Path = path,
            Resource = path,
            Body = body,
            Headers = Headers
        };
}

public class WebSocketMessageRequest
{
    public required string Path { get; init; }
    public required JsonElement Body { get; init; }
}