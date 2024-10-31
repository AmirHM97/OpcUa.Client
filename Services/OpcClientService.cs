namespace OpcUa.Client.Services;

public class OpcClientService
{
    private readonly IServerConnectionService _serverConnection;
    private readonly INodeBrowserService _nodeBrowser;
    private readonly ICircuitBreaker _circuitBreaker;
    private readonly Models.ServerConfiguration _serverConfiguration;

    public OpcClientService(IServerConnectionService serverConnection, INodeBrowserService nodeBrowser, ICircuitBreaker circuitBreaker, Models.ServerConfiguration serverConfiguration)
    {
        _serverConnection = serverConnection;
        _nodeBrowser = nodeBrowser;
        _circuitBreaker = circuitBreaker;
        _serverConfiguration = serverConfiguration;
    }
    public async Task RunAsync()
    {
        var tasks = _serverConfiguration.Servers.Select(RetrieveDataFromServerAsync);
        await Task.WhenAll(tasks);
    }

    public async Task RetrieveDataFromServerAsync(Server server)
    {
        using (Session session = await _serverConnection.ConnectAsync(server.Url))
        {
            Console.WriteLine($"Connected to OPC UA server: {server.Url}");

            var scalarFolderNodeId = await _nodeBrowser.FindFolderNodeIdAsync(session, ObjectIds.ObjectsFolder, server.FolderName);

            if (scalarFolderNodeId != null)
            {
                var variableNodeIds = await _nodeBrowser.GetVariableNodeIdsAsync(session, scalarFolderNodeId);

                Console.WriteLine($"Retrieved Variables from server {server.Url}:");
                foreach (var nodeId in variableNodeIds)
                {
                    var value = await session.ReadValueAsync(nodeId);
                    Console.WriteLine($"Server: {server.Url}, NodeId: {nodeId}, Value: {value.Value}");
                }
            }
            else
            {
                Console.WriteLine($"The Scalar folder could not be found on server: {server.Url}");
            }
        }
    }
}
