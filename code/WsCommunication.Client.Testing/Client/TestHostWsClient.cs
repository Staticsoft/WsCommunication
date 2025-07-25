﻿using Microsoft.AspNetCore.TestHost;
using Staticsoft.WsCommunication.Client.Abstractions;

namespace Staticsoft.WsCommunication.Client.Testing;

public class TestHostWsClient(
    WebSocketClient client
) : WsClient
{
    readonly WebSocketClient Client = client;

    public async Task<WsConnection> Connect()
    {
        var webSocket = await Client.ConnectAsync(new Uri("http://localhost"), CancellationToken.None);
        return new WsClientConnection(webSocket);
    }
}
