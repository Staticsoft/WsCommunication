using Staticsoft.Contracts.ASP.Server;
using Staticsoft.Serialization.Net;
using Staticsoft.TestContracts;
using System.Reflection;

namespace Staticsoft.TestServer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
        => RegisterServices(services);

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        => ConfigureApp(app, env);

    protected virtual IApplicationBuilder ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env) => app
        .UseRouting()
        .UseServerAPIRouting<Schema>()
        .UseEndpoints(Endpoints);

    protected virtual IServiceCollection RegisterServices(IServiceCollection services) => services
        .AddControllers().Services
        .UseServerAPI<Schema>(Assembly.GetExecutingAssembly())
        .UseSystemJsonSerializer();

    void Endpoints(IEndpointRouteBuilder endpoints)
        => ConfigureEndpoints(endpoints);

    protected virtual IEndpointRouteBuilder ConfigureEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllers();
        return endpoints;
    }
}
