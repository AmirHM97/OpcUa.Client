namespace OPCUA.Client.Services;

public class OpcClientService
{
    private readonly IServerConnectionService _serverConnection;
    private readonly INodeBrowserService _nodeBrowser;

    public OpcClientService(IServerConnectionService serverConnection, INodeBrowserService nodeBrowser)
    {
        _serverConnection = serverConnection;
        _nodeBrowser = nodeBrowser;
    }

    public async Task RunAsync(string folderName)
    {
        using (Session session = await _serverConnection.ConnectAsync())
        {
            Console.WriteLine("Connected to OPC UA server.");

           var scalarFolderNodeId = await _nodeBrowser.FindFolderNodeIdAsync(session, ObjectIds.ObjectsFolder, folderName);

            if (scalarFolderNodeId != null)
            {
                var variableNodeIds = await _nodeBrowser.GetVariableNodeIdsAsync(session, scalarFolderNodeId);

                Console.WriteLine("Retrieved Variables:");
                foreach (var nodeId in variableNodeIds)
                {
                   var value = await session.ReadValueAsync(nodeId);
                    Console.WriteLine($"NodeId: {nodeId}, Value: {value.Value}");
                }
            }
            else
            {
                Console.WriteLine("The scalar folder could not be found.");
            }
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
