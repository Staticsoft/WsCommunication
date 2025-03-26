using Staticsoft.WsCommunication.Client.Abstractions;
using System.Net.WebSockets;

namespace Staticsoft.WsCommunication.Client.Remote;

public class RemoteWsClient(
    RemoteWsClient.Options options
) : WsClient
{
    public class Options
    {
        public required string Uri { get; init; }
    }

    readonly Options Configuration = options;
    readonly ClientWebSocket Client = new();

    public async Task<WsConnection> Connect()
    {
        await Client.ConnectAsync(new Uri(Configuration.Uri), CancellationToken.None);
        return new WsConnection(Client);
    }
}
