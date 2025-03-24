using Amazon.ApiGatewayManagementApi;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.WsCommunication.Server.ApiGateway;

public class ApiGatewayWsServer(
    AmazonApiGatewayManagementApiClient client
) : WsServer
{
    readonly AmazonApiGatewayManagementApiClient Client = client;

    public async Task Send(string connectionId, byte[] data)
    {
        using var memoryStream = new MemoryStream(data);
        await Client.PostToConnectionAsync(new() { ConnectionId = connectionId, Data = memoryStream });
    }
}
