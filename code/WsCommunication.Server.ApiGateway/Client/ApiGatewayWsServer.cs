using Amazon.ApiGatewayManagementApi;
using Staticsoft.WsCommunication.Server.Abstractions;
using System.Text;
using System.Text.Json;

namespace Staticsoft.WsCommunication.Server.ApiGateway;

public class ApiGatewayWsServer(
    AmazonApiGatewayManagementApiClient client
) : WsServer
{
    readonly AmazonApiGatewayManagementApiClient Client = client;

    public async Task Send<T>(string connectionId, T message)
    {
        var outMessage = new WsServerOutMessage<T>()
        {
            Type = typeof(T).Name,
            Body = message
        };
        var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(outMessage));
        using var memoryStream = new MemoryStream(data);
        await Client.PostToConnectionAsync(new() { ConnectionId = connectionId, Data = memoryStream });
    }
}
