using Staticsoft.Contracts.Abstractions;
using Staticsoft.WsCommunication.Server.Abstractions;

namespace Staticsoft.TestContracts;

public class WebSocket(
    HttpEndpoint<WsServerInMessage<TestMessage>, TestMessageResponse> testMessage
)
{
    [Endpoint(HttpMethod.Post)]
    public HttpEndpoint<WsServerInMessage<TestMessage>, TestMessageResponse> TestMessage { get; } = testMessage;
}

public class TestMessage
{
    public required string TestProperty { get; init; }
}

public class TestMessageResponse { }