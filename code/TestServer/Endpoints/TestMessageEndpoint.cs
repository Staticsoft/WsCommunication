using Staticsoft.Contracts.Abstractions;
using Staticsoft.TestContracts;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.TestServer;

public class TestMessageEndpoint(
    WsServer ws
) : HttpEndpoint<WsServerInMessage<TestMessage>, TestMessageResponse>
{
    readonly WsServer Ws = ws;

    public async Task<TestMessageResponse> Execute(WsServerInMessage<TestMessage> request)
    {
        await Ws.Send(request.ConnectionId, request.Body);
        return new();
    }
}
