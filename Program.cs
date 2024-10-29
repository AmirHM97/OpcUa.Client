public static class Program
{
    private static readonly string ServerUrl = "opc.tcp://Amirs-Zephyrus-G15:48020";
    private static readonly string folderName = "Scalar";

    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<IServerConnectionService>(new ServerConnectionService(ServerUrl));
        services.AddSingleton<INodeBrowserService, NodeBrowserService>();
        services.AddSingleton<OpcClientService>();

        var serviceProvider = services.BuildServiceProvider();
        var clientApp = serviceProvider.GetService<OpcClientService>();
        await clientApp.RunAsync(folderName);
    }
}