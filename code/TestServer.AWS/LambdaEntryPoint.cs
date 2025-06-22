using Amazon.Lambda.Core;
using Staticsoft.Asp.Lambda;
using Staticsoft.WsCommunication.Server.Lambda;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Staticsoft.TestServer.AWS;

public class LambdaEntryPoint : LambdaEntrypoint<AWSStartup>
{
    protected override TriggerCollection Triggers => base.Triggers
        .AddTrigger<CloudWatchTrigger>()
        .AddTrigger<RequestTrigger>()
        .AddTrigger<WebSocketTrigger>();
}
