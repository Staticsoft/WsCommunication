using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Staticsoft.Asp.Lambda;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Staticsoft.TestServer.AWS;

public class LambdaEntryPoint : LambdaEntrypoint<AWSStartup>
{
    protected override TriggerCollection Triggers => base.Triggers
        .AddTrigger<CloudWatchTrigger>()
        .AddTrigger<RequestTrigger>()
        .AddTrigger<WebSocketTrigger>();
}

public class WebSocketTrigger : TriggerSource
{
    public bool TryConvert(
        JsonElement request,
        JsonSerializerOptions options,
        [NotNullWhen(true)] out APIGatewayProxyRequest? proxyRequest
    )
    {
        Console.WriteLine(request.GetRawText());
        proxyRequest = null;
        return false;
    }
}