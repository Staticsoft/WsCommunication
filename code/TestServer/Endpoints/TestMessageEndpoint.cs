using Staticsoft.Contracts.Abstractions;
using Staticsoft.TestContracts;
using Staticsoft.WsCommunication.Server.Abstractions;
using System.Text;

namespace Staticsoft.TestServer;

public class TestMessageEndpoint(
    WsManagement ws
) : HttpEndpoint<WsServerInMessage<TestMessage>, TestMessageResponse>
{
    readonly WsManagement Ws = ws;

    public async Task<TestMessageResponse> Execute(WsServerInMessage<TestMessage> request)
    {
        await Ws.Send(request.ConnectionId, Encoding.UTF8.GetBytes(request.Body.TestProperty));
        return new();
    }
}
