using Amazon.Lambda.APIGatewayEvents;
using Microsoft.AspNetCore.Authentication;
using Staticsoft.Asp.Lambda;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Staticsoft.WsCommunication.Server.Lambda;

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

        if (request.TryGetProperty("requestContext", out var context))
        {
            proxyRequest = (context.GetString("eventType"), context.GetString("connectionId")) switch
            {
                ("CONNECT", string id)
                    when context.TryGetProperty("authorizer", out var authorizer) => ConnectRequest(id, ParseClaims(authorizer)),
                ("CONNECT", string id) => ConnectRequest(id, []),
                ("DISCONNECT", string id) => DisconnectRequest(id),
                ("MESSAGE", string id) when request.GetString("body") is string body => MessageRequest(id, body),
                _ => null
            };
        }

        return proxyRequest != null;
    }

    static APIGatewayProxyRequest ConnectRequest(string connectionId, Dictionary<string, string> claims)
        => Request("/WebSocket/Connect", $@"{{""ConnectionId"":""{connectionId}""}}", claims);

    static Dictionary<string, string> ParseClaims(JsonElement authorizer)
    {
        const string claimsProperty = "claims.";
        var claims = new Dictionary<string, string>();
        foreach (var property in authorizer.EnumerateObject())
        {
            if (property.Name.StartsWith(claimsProperty) && property.Value.GetString() is string value)
            {
                claims.Add(property.Name[claimsProperty.Length..], value);
            }
        }
        return claims;
    }

    static APIGatewayProxyRequest DisconnectRequest(string connectionId)
        => Request("/WebSocket/Disconnect", $@"{{""ConnectionId"":""{connectionId}""}}", []);

    static APIGatewayProxyRequest? MessageRequest(string connectionId, string body)
        => JsonSerializer.Deserialize<Message>(body) switch
        {
            Message request => MessageRequest(connectionId, request),
            _ => null
        };

    static APIGatewayProxyRequest MessageRequest(string connectionId, Message request)
        => Request(request.Path, $@"{{""ConnectionId"":""{connectionId}"",""Body"":{request.Body.GetRawText()}}}", []);

    static APIGatewayProxyRequest Request(string path, string body, Dictionary<string, string> claims)
        => new()
        {
            RequestContext = new()
            {
                Authorizer = new()
                {
                    Claims = claims
                }
            },
            HttpMethod = "POST",
            Path = path,
            Resource = path,
            Body = body,
            Headers = Headers
        };

    class Message
    {
        public required string Path { get; init; }
        public required JsonElement Body { get; init; }
    }
}
