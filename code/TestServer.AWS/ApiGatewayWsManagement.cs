using Amazon.ApiGatewayManagementApi;

namespace Staticsoft.TestServer.AWS;

public class ApiGatewayWsManagement(
    AmazonApiGatewayManagementApiClient client
) : WsManagement
{
    readonly AmazonApiGatewayManagementApiClient Client = client;

    public async Task Send(string connectionId, byte[] data)
    {
        using var memoryStream = new MemoryStream(data);
        await Client.PostToConnectionAsync(new() { ConnectionId = connectionId, Data = memoryStream });
    }
}

