public static class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        services.AddCoreServices(configuration);

        var serviceProvider = services.BuildServiceProvider();
        var clientApp = serviceProvider.GetService<OpcClientService>();
        await clientApp.RunAsync();
    }
}