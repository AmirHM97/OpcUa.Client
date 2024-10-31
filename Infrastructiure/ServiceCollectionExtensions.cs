namespace OpcUa.Client.Infrastructiure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration)
                .AddSingleton<IServerConnectionService, ServerConnectionService>()
                .AddSingleton<ICircuitBreaker>(new CircuitBreaker(failureThreshold: 3, openTimeout: TimeSpan.FromMinutes(5)))
                .AddSingleton<INodeBrowserService, NodeBrowserService>()
                .AddSingleton<OpcClientService>();

        var config = new Models.ServerConfiguration();
        configuration.GetSection("ConnectionStrings").Bind(config);
        services.AddSingleton(config);
        return services;
    }
}
