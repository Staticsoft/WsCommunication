using Staticsoft.WsCommunication.Server.Local;

namespace Staticsoft.TestServer.Local;

public class LocalStartup : Startup
{
    protected override IServiceCollection RegisterServices(IServiceCollection services)
        => base.RegisterServices(services)
            .UseLocalWsCommunication();

    protected override IApplicationBuilder ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        => base.ConfigureApp(app, env)
            .UseLocalWsCommunication();
}