using Amazon.ApiGatewayManagementApi;
using Staticsoft.WsCommunication.Server.Abstractions;
using System.Text;

namespace Staticsoft.WsCommunication.Server.ApiGateway;

public class ApiGatewayWsServer(
    AmazonApiGatewayManagementApiClient client
) : WsServer
{
    readonly AmazonApiGatewayManagementApiClient Client = client;

    public async Task Send(string connectionId, string message)
    {
        var data = Encoding.UTF8.GetBytes(message);
        using var memoryStream = new MemoryStream(data);
        await Client.PostToConnectionAsync(new() { ConnectionId = connectionId, Data = memoryStream });
    }
}
