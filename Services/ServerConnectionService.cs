namespace OPCUA.Client.Services;

public class ServerConnectionService : IServerConnectionService
{
    private readonly string _serverUrl;

    public ServerConnectionService(string serverUrl)
    {
        _serverUrl = serverUrl;
    }

    public async Task<Session> ConnectAsync()
    {
        var application = new ApplicationInstance
        {
            ApplicationName = "OpcUaClientApp",
            ApplicationType = ApplicationType.Client
        };

        await application.LoadApplicationConfiguration(false);
        await application.CheckApplicationInstanceCertificate(false, 0);

        var selectedEndpoint = CoreClientUtils.SelectEndpoint(_serverUrl, useSecurity: false);
        var endpointConfig = EndpointConfiguration.Create(application.ApplicationConfiguration);
        var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfig);

        return await Session.Create(
            application.ApplicationConfiguration,
            endpoint,
            false,
            "OpcUaClientAppSession",
            60000,
            null,
            null);
    }
}
